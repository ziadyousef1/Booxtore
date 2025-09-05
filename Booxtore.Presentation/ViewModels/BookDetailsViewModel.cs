using Booxtore.Domain.Models;

namespace Booxtore.Presentation.ViewModels
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; } = null!;
        public List<Book> RelatedBooks { get; set; } = new List<Book>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
        public BorrowingRecord? CurrentUserBorrowing { get; set; }
    }
}
