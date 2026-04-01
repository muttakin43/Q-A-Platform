using Microsoft.AspNetCore.Mvc;

namespace Q_A.web.Controllers
{
    public class VoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
