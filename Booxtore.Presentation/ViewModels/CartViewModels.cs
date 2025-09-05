using Booxtore.Domain.Models;

namespace Booxtore.Presentation.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }

    public class CheckoutViewModel
    {
        public Cart Cart { get; set; } = new Cart();
        public ApplicationUser? User { get; set; }
        public decimal TotalAmount { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PaymentMethod { get; set; }
    }

    public class OrderSuccessViewModel
    {
        public PurchaseOrder Order { get; set; } = new PurchaseOrder();
    }

    public class OrderHistoryViewModel
    {
        public IEnumerable<PurchaseOrder> Orders { get; set; } = new List<PurchaseOrder>();
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
