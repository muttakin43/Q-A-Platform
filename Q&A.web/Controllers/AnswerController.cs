using Microsoft.AspNetCore.Mvc;

namespace Q_A.web.Controllers
{
    public class AnswerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
