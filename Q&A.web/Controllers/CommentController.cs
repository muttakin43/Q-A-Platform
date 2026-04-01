using Microsoft.AspNetCore.Mvc;

namespace Q_A.web.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
