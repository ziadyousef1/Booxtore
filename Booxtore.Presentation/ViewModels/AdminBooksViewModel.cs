using Booxtore.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booxtore.Presentation.ViewModels
{
    public class AdminBooksViewModel
    {
        public IEnumerable<Book> Books { get; set; } = new List<Book>();
        public string SearchTerm { get; set; } = string.Empty;
        public int? CategoryFilter { get; set; }
        public int? AuthorFilter { get; set; }
        public string StatusFilter { get; set; } = string.Empty;
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BorrowedBooks { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalPages { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Authors { get; set; } = new List<SelectListItem>();
    }

    public class EditBookViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string? Isbn { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool IsFree { get; set; } = false;
        public bool IsAvailableForBorrow { get; set; } = true;
        public bool IsAvailableForPurchase { get; set; } = true;
        public int? TotalCopies { get; set; }
        public int? AvailableCopies { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? PdfFileUrl { get; set; }
        public int? Pages { get; set; }
        public DateOnly? PublicationDate { get; set; }
        public string? Status { get; set; }
        public IFormFile? CoverImage { get; set; }
        public IFormFile? PdfFile { get; set; }
        public IEnumerable<SelectListItem> Authors { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }

    public class BookStatsViewModel
    {
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BorrowedBooks { get; set; }
        public int FreeBooks { get; set; }
        public int PaidBooks { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<CategoryStats> CategoryStats { get; set; } = new List<CategoryStats>();
        public List<PopularBook> PopularBooks { get; set; } = new List<PopularBook>();
    }

    public class CategoryStats
    {
        public string CategoryName { get; set; } = null!;
        public int BookCount { get; set; }
        public int BorrowCount { get; set; }
    }

    public class PopularBook
    {
        public string Title { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public int BorrowCount { get; set; }
        public int ReviewCount { get; set; }
        public double AverageRating { get; set; }
    }

    public class CreateAuthorViewModel
    {
        public string Name { get; set; } = null!;
        public string? Biography { get; set; }
        public DateOnly? BirthDate { get; set; }
    }

    public class CreateCategoryViewModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
