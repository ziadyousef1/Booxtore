using Booxtore.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booxtore.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IBookService bookService,
            IUserService userService,
            ILogger<AdminController> logger)
        {
            _bookService = bookService;
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TotalBooks = await _bookService.GetTotalBooksCountAsync();
                ViewBag.AvailableBooks = await _bookService.GetAvailableBooksCountAsync();
                ViewBag.BorrowedBooks = await _bookService.GetBorrowedBooksCountAsync();
                ViewBag.TotalUsers = await _userService.GetTotalUsersAsync();
                ViewBag.ActiveUsers = await _userService.GetActiveUsersAsync();
                ViewBag.NewUsersThisMonth = await _userService.GetNewUsersThisMonthAsync();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading admin dashboard");
                TempData["Error"] = "An error occurred while loading the dashboard.";
                return View();
            }
        }
    }
}
