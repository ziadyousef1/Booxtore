using Microsoft.AspNetCore.Mvc;
using Booxtore.Application.Interfaces.Services;
using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Presentation.ViewModels;
using Booxtore.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Booxtore.Presentation.Controllers
{
    public class ShopController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBorrowingRecordRepository _borrowingRecordRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShopController(
            IBookService bookService,
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository,
            IBorrowingRecordRepository borrowingRecordRepository,
            UserManager<ApplicationUser> userManager)
        {
            _bookService = bookService;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _borrowingRecordRepository = borrowingRecordRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? categoryId, int? authorId, string search, int page = 1, int pageSize = 12)
        {
            IEnumerable<Book> books;

            if (!string.IsNullOrEmpty(search))
            {
                books = await _bookService.SearchBooksAsync(search);
            }
            else if (categoryId.HasValue)
            {
                books = await _bookService.GetBooksByCategoryAsync(categoryId.Value);
            }
            else if (authorId.HasValue)
            {
                books = await _bookService.GetBooksByAuthorAsync(authorId.Value);
            }
            else
            {
                books = await _bookService.GetAllBooksAsync();
            }

            books = books.Where(b => b.Status == "Available" && 
                               (b.IsAvailableForPurchase == true || b.IsAvailableForBorrow == true));

            var totalBooks = books.Count();
            var totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
            
            var pagedBooks = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Check if current user has borrowed any of these books
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    foreach (var book in pagedBooks)
                    {
                        var activeBorrowing = await _borrowingRecordRepository.GetActiveByUserAndBookAsync(user.Id, book.BookId);
                        book.IsBorrowedByCurrentUser = activeBorrowing != null;
                    }
                }
            }

            var viewModel = new ShopViewModel
            {
                Books = pagedBooks,
                Categories = (await _categoryRepository.GetAllAsync()).ToList(),
                Authors = (await _authorRepository.GetAllAsync()).ToList(),
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalBooks = totalBooks,
                SelectedCategoryId = categoryId,
                SelectedAuthorId = authorId,
                SearchTerm = search
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var relatedBooks = book.CategoryId.HasValue
                ? (await _bookService.GetBooksByCategoryAsync(book.CategoryId.Value))
                    .Where(b => b.BookId != id && b.Status == "Available")
                    .Take(4)
                    .ToList()
                : new List<Book>();

            BorrowingRecord? currentBorrowing = null;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    currentBorrowing = await _borrowingRecordRepository.GetActiveByUserAndBookAsync(user.Id, book.BookId);
                    book.IsBorrowedByCurrentUser = currentBorrowing != null;
                    
                    // Also check for related books
                    foreach (var relatedBook in relatedBooks)
                    {
                        var relatedBorrowing = await _borrowingRecordRepository.GetActiveByUserAndBookAsync(user.Id, relatedBook.BookId);
                        relatedBook.IsBorrowedByCurrentUser = relatedBorrowing != null;
                    }
                }
            }

            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                RelatedBooks = relatedBooks,
                CurrentUserBorrowing = currentBorrowing
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Read(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            if (book.IsFree != true)
            {
                TempData["Error"] = "This book is not available for free reading.";
                return RedirectToAction("Details", new { id = id });
            }

            if (string.IsNullOrEmpty(book.PdfFileUrl))
            {
                TempData["Error"] = "PDF file is not available for this book.";
                return RedirectToAction("Details", new { id = id });
            }

            var viewModel = new BookReaderViewModel
            {
                Book = book,
                PdfPath = book.PdfFileUrl
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Borrow(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return Json(new { success = false, message = "Book not found." });
                }

                if (book.IsAvailableForBorrow != true || book.AvailableCopies <= 0)
                {
                    return Json(new { success = false, message = "Book is not available for borrowing." });
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                var existingBorrowing = await _borrowingRecordRepository.GetActiveByUserAndBookAsync(user.Id, book.BookId);
                if (existingBorrowing != null)
                {
                    return Json(new { success = false, message = "You have already borrowed this book. Please return it before borrowing again." });
                }

                var borrowingRecord = new BorrowingRecord
                {
                    BookId = book.BookId,
                    UserId = user.Id,
                    BorrowDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(14),
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                };

                await _borrowingRecordRepository.AddAsync(borrowingRecord);
                
                book.AvailableCopies = book.AvailableCopies - 1;
                await _bookService.UpdateBookAsync(book);

                return Json(new { 
                    success = true, 
                    message = "Book borrowed successfully! You have 14 days to return it.",
                    readUrl = book.IsFree == true && !string.IsNullOrEmpty(book.PdfFileUrl) 
                        ? Url.Action("Read", new { id = book.BookId }) 
                        : null
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while borrowing the book." });
            }
        }

        public async Task<IActionResult> Category(int id, int page = 1, int pageSize = 12)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", new { categoryId = id, page = page, pageSize = pageSize });
        }

    }
}
