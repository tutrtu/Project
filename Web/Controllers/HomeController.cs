using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
