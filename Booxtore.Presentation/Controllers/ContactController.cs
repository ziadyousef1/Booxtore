using Microsoft.AspNetCore.Mvc;

namespace Booxtore.Presentation.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
