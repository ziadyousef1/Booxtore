using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Application.Interfaces.Services;
using Booxtore.Domain.Models;
using Booxtore.Presentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Booxtore.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminUsersController> _logger;

        public AdminUsersController(
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            ILogger<AdminUsersController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            try
            {
                IEnumerable<ApplicationUser> users;
                
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    users = await _userService.SearchUsersAsync(searchTerm);
                }
                else
                {
                    users = await _userService.GetAllUsersAsync();
                }

                var totalUsers = await _userService.GetTotalUsersAsync();
                var activeUsers = await _userService.GetActiveUsersAsync();

                var pagedUsers = users.Skip((page - 1) * pageSize).Take(pageSize);
                var totalPages = (int)Math.Ceiling(users.Count() / (double)pageSize);

                var viewModel = new AdminUsersViewModel
                {
                    Users = pagedUsers,
                    SearchTerm = searchTerm,
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUsers,
                    InactiveUsers = totalUsers - activeUsers,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading users");
                TempData["Error"] = "An error occurred while loading users.";
                return View(new AdminUsersViewModel());
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var viewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber ?? "",
                    Status = user.Status,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    TotalBorrowings = user.BorrowingRecords?.Count ?? 0,
                    TotalPurchases = user.PurchaseOrders?.Count ?? 0,
                    TotalReviews = user.Reviews?.Count ?? 0
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading user details for ID: {UserId}", id);
                TempData["Error"] = "An error occurred while loading user details.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var viewModel = new EditUserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber ?? "",
                    Status = user.Status,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading user for edit. ID: {UserId}", id);
                TempData["Error"] = "An error occurred while loading user for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.Status = model.Status;

                await _userService.UpdateUserAsync(user);

                TempData["Success"] = "User updated successfully.";
                return RedirectToAction(nameof(Details), new { id = model.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user. ID: {UserId}", model.Id);
                TempData["Error"] = "An error occurred while updating the user.";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser?.Id == id)
                {
                    TempData["Error"] = "You cannot delete your own account.";
                    return RedirectToAction(nameof(Index));
                }

                var success = await _userService.DeleteUserAsync(id);
                if (success)
                {
                    TempData["Success"] = "User deleted successfully.";
                }
                else
                {
                    TempData["Error"] = "Failed to delete user.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user. ID: {UserId}", id);
                TempData["Error"] = "An error occurred while deleting the user.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Stats()
        {
            try
            {
                var totalUsers = await _userService.GetTotalUsersAsync();
                var activeUsers = await _userService.GetActiveUsersAsync();

                var viewModel = new UserStatsViewModel
                {
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUsers,
                    NewUsersThisMonth = 0, // TODO: Implement this logic
                    TotalBorrowings = 0, // TODO: Implement this logic
                    TotalPurchases = 0 // TODO: Implement this logic
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading user statistics");
                TempData["Error"] = "An error occurred while loading statistics.";
                return View(new UserStatsViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id, [FromBody] dynamic data)
        {
            try
            {
                string status = data.status;
                var result = await _userService.ToggleUserStatusAsync(id, status);
                
                if (result)
                {
                    return Json(new { success = true });
                }
                
                return Json(new { success = false, message = "User not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling user status for ID: {UserId}", id);
                return Json(new { success = false, message = "An error occurred" });
            }
        }
    }
}
