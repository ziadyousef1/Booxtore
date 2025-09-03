using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class PurchaseOrder
{
    public int OrderId { get; set; }

    public string? UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? PaymentStatus { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ApplicationUser? User { get; set; }
}
