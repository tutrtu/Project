using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class QuestionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
