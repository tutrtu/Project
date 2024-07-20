using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Web.Models;
using static System.Net.WebRequestMethods;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<List<QuestionViewModel>>("http://localhost:5228/api/Questions");

                if (response != null)
                {
                    return View(response);
                }
                else
                {
                    _logger.LogWarning("API returned null or empty response.");
                    return View(new List<QuestionViewModel>());
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching questions: {ex.Message}");
                return RedirectToAction("Error");
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<ActionResult> Categories()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<List<CategoryDto>>("http://localhost:5228/api/Categories");

                if (response != null)
                {
                    return View(response);
                }
                else
                {
                    _logger.LogWarning("API returned null or empty response.");
                    return View(new List<CategoryDto>());
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching questions: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto category)
        {
            try
            {
                // Check if the user is logged in
                int? userId = HttpContext.Session.GetInt32("CurrentUserId");
                if (userId == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Set the User ID and current date/time
                
                category.CategoryID = 1;

                // Fetch categories from API
                var client = _clientFactory.CreateClient();
              

               

                // Prepare the categories for the view
              

                // Post the question to the API
                var response = await client.PostAsJsonAsync("http://localhost:5228/api/Categories", category);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Categories", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to create question.";
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
                return View(category);
            }
        }


        public async Task<IActionResult> Search(string str)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var apiUrl = $"http://localhost:5228/api/Questions/{str}";
                var questions = await client.GetFromJsonAsync<List<QuestionViewModel>>(apiUrl);
                ViewBag.str = str;
                if (questions != null)
                {
                    return View(questions);
                }
                else
                {
                    _logger.LogWarning("API returned null or empty response for search.");
                    return View(new List<QuestionViewModel>());
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching search results: {ex.Message}");
                return RedirectToAction("Error");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
