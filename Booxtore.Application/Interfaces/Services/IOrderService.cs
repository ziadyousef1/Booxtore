using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<PurchaseOrder> CreateOrderAsync(string userId, Cart cart);
        Task<PurchaseOrder> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<PurchaseOrder>> GetUserOrdersAsync(string userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string paymentStatus, string? paymentMethod = null);
        Task<bool> CompleteOrderAsync(int orderId);
    }
}
