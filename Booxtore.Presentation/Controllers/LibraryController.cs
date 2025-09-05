using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Application.Interfaces.Services;
using Booxtore.Domain.Models;
using Booxtore.Presentation.ViewModels;

namespace Booxtore.Presentation.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly IBorrowingRecordRepository _borrowingRecordRepository;
        private readonly IBookService _bookService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LibraryController(
            IBorrowingRecordRepository borrowingRecordRepository,
            IBookService bookService,
            UserManager<ApplicationUser> userManager)
        {
            _borrowingRecordRepository = borrowingRecordRepository;
            _bookService = bookService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var borrowingRecords = await _borrowingRecordRepository.GetByUserIdAsync(user.Id);
            
            var viewModel = new LibraryViewModel
            {
                ActiveBorrowings = borrowingRecords.Where(br => br.Status == "Active").ToList(),
                CompletedBorrowings = borrowingRecords.Where(br => br.Status == "Returned").ToList(),
                OverdueBorrowings = borrowingRecords.Where(br => br.Status == "Active" && br.DueDate < DateTime.UtcNow).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Return(int borrowingRecordId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var borrowingRecord = await _borrowingRecordRepository.GetByIdAsync(borrowingRecordId);

                if (borrowingRecord == null || borrowingRecord.UserId != user.Id)
                {
                    return Json(new { success = false, message = "Borrowing record not found or access denied." });
                }

                if (borrowingRecord.Status != "Active")
                {
                    return Json(new { success = false, message = "This book has already been returned." });
                }

                borrowingRecord.Status = "Returned";
                borrowingRecord.ReturnDate = DateTime.UtcNow;
                await _borrowingRecordRepository.UpdateAsync(borrowingRecord);

                var book = await _bookService.GetBookByIdAsync(borrowingRecord.BookId.Value);
                if (book != null)
                {
                    book.AvailableCopies++;
                    await _bookService.UpdateBookAsync(book);
                }

                return Json(new { success = true, message = "Book returned successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while returning the book." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Renew(int borrowingRecordId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var borrowingRecord = await _borrowingRecordRepository.GetByIdAsync(borrowingRecordId);

                if (borrowingRecord == null || borrowingRecord.UserId != user.Id)
                {
                    return Json(new { success = false, message = "Borrowing record not found or access denied." });
                }

                if (borrowingRecord.Status != "Active")
                {
                    return Json(new { success = false, message = "Cannot renew a returned book." });
                }

                if (borrowingRecord.DueDate.HasValue && borrowingRecord.DueDate.Value > DateTime.UtcNow.AddDays(7))
                {
                    return Json(new { success = false, message = "Book can only be renewed if due within 7 days." });
                }

                if (borrowingRecord.DueDate.HasValue)
                {
                    borrowingRecord.DueDate = borrowingRecord.DueDate.Value.AddDays(14);
                }
                else
                {
                    borrowingRecord.DueDate = DateTime.UtcNow.AddDays(14);
                }
                
                await _borrowingRecordRepository.UpdateAsync(borrowingRecord);

                return Json(new { success = true, message = "Book renewed successfully! New due date: " + borrowingRecord.DueDate?.ToString("MMM dd, yyyy") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while renewing the book." });
            }
        }
    }
}
