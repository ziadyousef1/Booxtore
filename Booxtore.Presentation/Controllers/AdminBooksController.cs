using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Application.Interfaces.Services;
using Booxtore.Domain.Models;
using Booxtore.Presentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booxtore.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<AdminBooksController> _logger;

        public AdminBooksController(
            IBookService bookService,
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository,
            IWebHostEnvironment environment,
            ILogger<AdminBooksController> logger)
        {
            _bookService = bookService;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
            _environment = environment;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchTerm = "", int? categoryFilter = null, int? authorFilter = null, string statusFilter = "", int page = 1, int pageSize = 12)
        {
            try
            {
                IEnumerable<Book> books;
                
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    books = await _bookService.SearchBooksAsync(searchTerm);
                }
                else
                {
                    books = await _bookService.GetAllBooksAsync();
                }

                // Apply filters
                if (categoryFilter.HasValue)
                {
                    books = books.Where(b => b.CategoryId == categoryFilter.Value);
                }

                if (authorFilter.HasValue)
                {
                    books = books.Where(b => b.AuthorId == authorFilter.Value);
                }

                if (!string.IsNullOrEmpty(statusFilter))
                {
                    books = books.Where(b => b.Status == statusFilter);
                }

                var categories = await _categoryRepository.GetAllAsync();
                var authors = await _authorRepository.GetAllAsync();

                var pagedBooks = books.Skip((page - 1) * pageSize).Take(pageSize);
                var totalPages = (int)Math.Ceiling(books.Count() / (double)pageSize);

                var viewModel = new AdminBooksViewModel
                {
                    Books = pagedBooks,
                    SearchTerm = searchTerm,
                    CategoryFilter = categoryFilter,
                    AuthorFilter = authorFilter,
                    StatusFilter = statusFilter,
                    TotalBooks = await _bookService.GetTotalBooksCountAsync(),
                    AvailableBooks = await _bookService.GetAvailableBooksCountAsync(),
                    BorrowedBooks = await _bookService.GetBorrowedBooksCountAsync(),
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    Categories = categories.Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.Name
                    }),
                    Authors = authors.Select(a => new SelectListItem
                    {
                        Value = a.AuthorId.ToString(),
                        Text = a.Name
                    })
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading books");
                TempData["Error"] = "An error occurred while loading books.";
                return View(new AdminBooksViewModel());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading book details for ID: {BookId}", id);
                TempData["Error"] = "An error occurred while loading book details.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var authors = await _authorRepository.GetAllAsync();

                var viewModel = new EditBookViewModel
                {
                    Categories = categories.Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.Name
                    }),
                    Authors = authors.Select(a => new SelectListItem
                    {
                        Value = a.AuthorId.ToString(),
                        Text = a.Name
                    })
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading create book page");
                TempData["Error"] = "An error occurred while loading the page.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists(model);
                return View(model);
            }

            try
            {
                var book = new Book
                {
                    Title = model.Title,
                    AuthorId = model.AuthorId,
                    CategoryId = model.CategoryId,
                    Isbn = model.Isbn,
                    Description = model.Description,
                    Price = model.Price,
                    IsFree = model.IsFree,
                    IsAvailableForBorrow = model.IsAvailableForBorrow,
                    IsAvailableForPurchase = model.IsAvailableForPurchase,
                    TotalCopies = model.TotalCopies,
                    AvailableCopies = model.AvailableCopies,
                    Pages = model.Pages,
                    PublicationDate = model.PublicationDate,
                    Status = model.Status ?? "Available",
                    CreatedAt = DateTime.UtcNow
                };

                // Handle file uploads
                if (model.CoverImage != null)
                {
                    book.CoverImageUrl = await SaveFileAsync(model.CoverImage, "covers");
                }

                if (model.PdfFile != null)
                {
                    book.PdfFileUrl = await SaveFileAsync(model.PdfFile, "pdfs");
                }

                await _bookService.CreateBookAsync(book);

                TempData["Success"] = "Book created successfully.";
                return RedirectToAction(nameof(Details), new { id = book.BookId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating book");
                TempData["Error"] = "An error occurred while creating the book.";
                await LoadSelectLists(model);
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                var viewModel = new EditBookViewModel
                {
                    BookId = book.BookId,
                    Title = book.Title,
                    AuthorId = book.AuthorId,
                    CategoryId = book.CategoryId,
                    Isbn = book.Isbn,
                    Description = book.Description,
                    Price = book.Price,
                    IsFree = book.IsFree ?? false,
                    IsAvailableForBorrow = book.IsAvailableForBorrow ?? true,
                    IsAvailableForPurchase = book.IsAvailableForPurchase ?? true,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    CoverImageUrl = book.CoverImageUrl,
                    PdfFileUrl = book.PdfFileUrl,
                    Pages = book.Pages,
                    PublicationDate = book.PublicationDate,
                    Status = book.Status
                };

                await LoadSelectLists(viewModel);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading book for edit. ID: {BookId}", id);
                TempData["Error"] = "An error occurred while loading book for editing.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists(model);
                return View(model);
            }

            try
            {
                var book = await _bookService.GetBookByIdAsync(model.BookId);
                if (book == null)
                {
                    return NotFound();
                }

                book.Title = model.Title;
                book.AuthorId = model.AuthorId;
                book.CategoryId = model.CategoryId;
                book.Isbn = model.Isbn;
                book.Description = model.Description;
                book.Price = model.Price;
                book.IsFree = model.IsFree;
                book.IsAvailableForBorrow = model.IsAvailableForBorrow;
                book.IsAvailableForPurchase = model.IsAvailableForPurchase;
                book.TotalCopies = model.TotalCopies;
                book.AvailableCopies = model.AvailableCopies;
                book.Pages = model.Pages;
                book.PublicationDate = model.PublicationDate;
                book.Status = model.Status;
                book.UpdatedAt = DateTime.UtcNow;

                // Handle file uploads
                if (model.CoverImage != null)
                {
                    if (!string.IsNullOrEmpty(book.CoverImageUrl))
                    {
                        DeleteFile(book.CoverImageUrl);
                    }
                    book.CoverImageUrl = await SaveFileAsync(model.CoverImage, "covers");
                }

                if (model.PdfFile != null)
                {
                    if (!string.IsNullOrEmpty(book.PdfFileUrl))
                    {
                        DeleteFile(book.PdfFileUrl);
                    }
                    book.PdfFileUrl = await SaveFileAsync(model.PdfFile, "pdfs");
                }

                await _bookService.UpdateBookAsync(book);

                TempData["Success"] = "Book updated successfully.";
                return RedirectToAction(nameof(Details), new { id = model.BookId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating book. ID: {BookId}", model.BookId);
                TempData["Error"] = "An error occurred while updating the book.";
                await LoadSelectLists(model);
                return View(model);
            }
        }

        public async Task<IActionResult> CreateAuthor()
        {
            return View(new CreateAuthorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor(CreateAuthorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var author = new Author
                {
                    Name = model.Name,
                    Biography = model.Biography,
                    BirthDate = model.BirthDate,
                    CreatedAt = DateTime.UtcNow
                };

                await _authorRepository.AddAsync(author);

                TempData["Success"] = "Author created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating author");
                TempData["Error"] = "An error occurred while creating the author.";
                return View(model);
            }
        }

        public async Task<IActionResult> CreateCategory()
        {
            return View(new CreateCategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var category = new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                    CreatedAt = DateTime.UtcNow
                };

                await _categoryRepository.AddAsync(category);

                TempData["Success"] = "Category created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category");
                TempData["Error"] = "An error occurred while creating the category.";
                return View(model);
            }
        }

        private async Task LoadSelectLists(EditBookViewModel model)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var authors = await _authorRepository.GetAllAsync();

            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            });

            model.Authors = authors.Select(a => new SelectListItem
            {
                Value = a.AuthorId.ToString(),
                Text = a.Name
            });
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folder}/{fileName}";
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return Json(new { success = false, message = "Book not found" });
                }

                // Delete associated files
                if (!string.IsNullOrEmpty(book.CoverImageUrl))
                {
                    DeleteFile(book.CoverImageUrl);
                }
                
                if (!string.IsNullOrEmpty(book.PdfFileUrl))
                {
                    DeleteFile(book.PdfFileUrl);
                }

                var result = await _bookService.DeleteBookAsync(id);
                
                if (result)
                {
                    return Json(new { success = true });
                }
                
                return Json(new { success = false, message = "Failed to delete book" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting book with ID: {BookId}", id);
                return Json(new { success = false, message = "An error occurred while deleting the book" });
            }
        }

        private void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
