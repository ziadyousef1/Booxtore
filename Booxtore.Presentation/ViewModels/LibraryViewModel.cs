using Booxtore.Domain.Models;

namespace Booxtore.Presentation.ViewModels
{
    public class LibraryViewModel
    {
        public List<BorrowingRecord> ActiveBorrowings { get; set; } = new List<BorrowingRecord>();
        public List<BorrowingRecord> CompletedBorrowings { get; set; } = new List<BorrowingRecord>();
        public List<BorrowingRecord> OverdueBorrowings { get; set; } = new List<BorrowingRecord>();
    }
}
