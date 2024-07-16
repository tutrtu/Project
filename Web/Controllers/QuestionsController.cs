using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Web.Models;

namespace Web.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public QuestionsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewQuestionViewModel questionViewModel)
        {
            try
            {
                if (HttpContext.Session.GetInt32("CurrentUserId") == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                questionViewModel.UserID = HttpContext.Session.GetInt32("CurrentUserId").Value;
                questionViewModel.QuestionDateAndTime = DateTime.Now;

                var json = JsonSerializer.Serialize(questionViewModel);
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5228/api/Questions", questionViewModel);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Create");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to create question.";
                    return View(questionViewModel);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
                return View(questionViewModel);
            }
        }
        public async Task<ActionResult> Create()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                List<CategoryDto> categories = await client.GetFromJsonAsync<List<CategoryDto>>("http://localhost:5228/api/Categories");

                if (categories != null)
                {
                    // Map categories to SelectListItem for the dropdown
                   
                     ViewBag.Categories = categories ; // Ensure ViewBag.Categories is not null
                    return View();
                }
                else
                {
                    // Handle null response from the API
                    ViewBag.ErrorMessage = "Failed to fetch categories.";
                    return View(new QuestionViewModel());
                }
            }
            catch (HttpRequestException ex)
            {
                // Log or handle HTTP request exception
                ViewBag.ErrorMessage = $"Error fetching categories: {ex.Message}";
                return View(new QuestionViewModel());
            }
        }


    }
}
