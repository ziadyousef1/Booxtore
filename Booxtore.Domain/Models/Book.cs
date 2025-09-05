using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public int? AuthorId { get; set; }

    public int? CategoryId { get; set; }

    public string? Isbn { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public bool? IsFree { get; set; }

    public bool? IsAvailableForBorrow { get; set; }

    public bool? IsAvailableForPurchase { get; set; }

    public int? TotalCopies { get; set; }

    public int? AvailableCopies { get; set; }

    public string? CoverImageUrl { get; set; }

    public string? PdfFileUrl { get; set; }

    public int? Pages { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<BorrowingRecord> BorrowingRecords { get; set; } = new List<BorrowingRecord>();

    public virtual Category? Category { get; set; }

    // Property to check if current user has borrowed this book
    public bool IsBorrowedByCurrentUser { get; set; } = false;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ReadingSession> ReadingSessions { get; set; } = new List<ReadingSession>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<UserLibrary> UserLibraries { get; set; } = new List<UserLibrary>();
}
