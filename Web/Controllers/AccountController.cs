using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Web.Models;
using static System.Net.WebRequestMethods;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5228/api/User?email={model.Email}&pass={model.Password}");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var userJson = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(userJson);

                // Set session variable
                HttpContext.Session.SetInt32("CurrentUserId", user.Id);
                

                // Handle successful login, e.g., set session data or cookies
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Handle login failure
                ViewBag.ErrorMessage = "Invalid email or password";
                return View(model);
            }
        }



        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _clientFactory.CreateClient();
            var user = new User
            {
                Email = model.Email,
                Password = model.Password,
                Name = model.Name,
                Mobile = model.Mobile
            };

            var response = await client.PostAsJsonAsync("http://localhost:5228/api/User", user);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetInt32("CurrentUserId", user.Id);
                HttpContext.Session.SetString("CurrentUser", user.Name);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                ViewBag.Error = message;
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

