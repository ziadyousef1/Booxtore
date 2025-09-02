using Microsoft.AspNetCore.Mvc;

namespace Booxtore.Presentation.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
