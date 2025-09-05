using Booxtore.Domain.Models;

namespace Booxtore.Presentation.ViewModels
{
    public class HomeViewModel
    {
        public List<Book> FeaturedBooks { get; set; } = new List<Book>();
        public List<Book> BestSellingBooks { get; set; } = new List<Book>();
        public List<Book> LatestBooks { get; set; } = new List<Book>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public int TotalBooksCount { get; set; }
        public int TotalCategoriesCount { get; set; }
        public int TotalAuthorsCount { get; set; }
    }
}