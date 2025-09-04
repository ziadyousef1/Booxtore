using Booxtore.Domain.Models;

namespace Booxtore.Presentation.ViewModels
{
    public class AdminUsersViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public string SearchTerm { get; set; } = string.Empty;
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TotalBorrowings { get; set; }
        public int TotalPurchases { get; set; }
        public int TotalReviews { get; set; }
    }

    public class UserStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int TotalBorrowings { get; set; }
        public int TotalPurchases { get; set; }
        public List<MonthlyUserData> MonthlyData { get; set; } = new List<MonthlyUserData>();
    }

    public class MonthlyUserData
    {
        public string Month { get; set; } = null!;
        public int NewUsers { get; set; }
        public int ActiveUsers { get; set; }
    }
}
