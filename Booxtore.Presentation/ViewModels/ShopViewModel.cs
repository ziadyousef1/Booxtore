using Booxtore.Domain.Models;

namespace Booxtore.Presentation.ViewModels
{
    public class ShopViewModel
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Author> Authors { get; set; } = new List<Author>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 12;
        public int TotalBooks { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedAuthorId { get; set; }
        public string? SearchTerm { get; set; }
    }
}
