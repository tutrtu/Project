using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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
        public async Task<IActionResult> View(int id)
        {
            var client = _clientFactory.CreateClient();

            // Correctly format the URL to include the id
            string apiUrl = $"http://localhost:5228/api/Questions/{id}";

            // Call the API to get a single question
            var qvm = await client.GetFromJsonAsync<QuestionViewModel>(apiUrl);

            // Check if the question is found
            if (qvm == null)
            {
                return NotFound(); // Or handle the case where the question is not found
            }

            // Return the view with the question view model
            return View(qvm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewQuestionViewModel questionViewModel)
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
                questionViewModel.UserID = userId.Value;
                questionViewModel.QuestionDateAndTime = DateTime.Now;

                // Fetch categories from API
                var client = _clientFactory.CreateClient();
                var categories = await client.GetFromJsonAsync<List<CategoryDto>>("http://localhost:5228/api/Categories");

                if (categories == null)
                {
                    ViewBag.ErrorMessage = "Failed to fetch categories.";
                    return View(questionViewModel);
                }

                // Prepare the categories for the view
                ViewBag.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.CategoryName
                }).ToList();

                // Post the question to the API
                var response = await client.PostAsJsonAsync("http://localhost:5228/api/Questions", questionViewModel);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
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
                    var selectListItems = categories.Select(c => new SelectListItem
                    {
                        Value = c.CategoryID.ToString(),
                        Text = c.CategoryName
                    }).ToList();

                    ViewBag.Categories = selectListItems;
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

        [HttpPost]
        public async Task<ActionResult> AddAnswer(NewAnswerViewModel navm)
        {
            //currrent working user id
            navm.UserID = HttpContext.Session.GetInt32("CurrentUserId").Value;
            
            //taking system date and time
            navm.AnswerDateAndTime = DateTime.Now;
            //by default vote count is 0
            
            //checking model state is valid or not
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("http://localhost:5228/api/Answers", navm);
                //after adding answer we are redirecting to questions controller view page
                return RedirectToAction("View", "Questions", new { id = navm.QuestionID });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                //
               
                return View("View", navm);
            }
        }
    }
}
