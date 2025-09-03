using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class ReadingSession
{
    public int SessionId { get; set; }

    public string? UserId { get; set; }

    public int? BookId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? PagesRead { get; set; }

    public int? LastPage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual ApplicationUser? User { get; set; }
}
