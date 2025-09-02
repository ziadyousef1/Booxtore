using Microsoft.AspNetCore.Mvc;

namespace Booxtore.Presentation.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Category(string category)
        {
            ViewBag.Category = category;
            return View();
        }
    }
}
