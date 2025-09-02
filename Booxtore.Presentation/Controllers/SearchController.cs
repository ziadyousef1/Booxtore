using Microsoft.AspNetCore.Mvc;

namespace Booxtore.Presentation.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index(string q)
        {
            ViewBag.Query = q;
            return View();
        }
    }
}
