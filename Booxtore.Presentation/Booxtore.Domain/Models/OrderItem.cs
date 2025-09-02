using System;
using System.Collections.Generic;

namespace Booxtore.Domain.Models;

public partial class OrderItem
{
    public int ItemId { get; set; }

    public int? OrderId { get; set; }

    public int? BookId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Book? Book { get; set; }

    public virtual PurchaseOrder? Order { get; set; }
}
