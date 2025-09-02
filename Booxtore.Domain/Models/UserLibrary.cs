using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class UserLibrary
{
    public int LibraryId { get; set; }

    public int? UserId { get; set; }

    public int? BookId { get; set; }

    public string AccessType { get; set; } = null!;

    public DateTime? PurchaseDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
