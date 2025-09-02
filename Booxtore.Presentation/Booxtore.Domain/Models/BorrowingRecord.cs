using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class BorrowingRecord
{
    public int BorrowId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public DateTime? BorrowDate { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? Status { get; set; }

    public decimal? FineAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
