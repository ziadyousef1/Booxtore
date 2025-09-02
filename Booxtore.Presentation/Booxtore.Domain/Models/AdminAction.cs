using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class AdminAction
{
    public int ActionId { get; set; }

    public int? AdminId { get; set; }

    public string ActionType { get; set; } = null!;

    public string? TargetTable { get; set; }

    public int? TargetId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? Admin { get; set; }
}
