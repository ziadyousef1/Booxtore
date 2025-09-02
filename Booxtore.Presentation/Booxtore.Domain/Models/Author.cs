using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string Name { get; set; } = null!;

    public string? Biography { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
