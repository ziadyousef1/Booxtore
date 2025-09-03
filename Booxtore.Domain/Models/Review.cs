using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public string? UserId { get; set; }

    public int? BookId { get; set; }

    public int? Rating { get; set; }

    public string? ReviewText { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
