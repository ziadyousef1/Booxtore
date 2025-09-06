using Booxtore.Application.Interfaces.Services;
using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;

namespace Booxtore.Application.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PurchaseOrder> CreateOrderAsync(string userId, Cart cart)
        {
            if (cart.Items.Count == 0)
                throw new InvalidOperationException("Cannot create order with empty cart");

            var order = new PurchaseOrder
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.TotalAmount,
                PaymentStatus = "Pending",
                PaymentMethod = null,
                OrderItems = cart.Items.Select(item => new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalPrice = item.Total
                }).ToList()
            };

            return await _orderRepository.AddAsync(order);
        }

        public async Task<PurchaseOrder> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<IEnumerable<PurchaseOrder>> GetUserOrdersAsync(string userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string paymentStatus, string? paymentMethod = null)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            order.PaymentStatus = paymentStatus;
            if (!string.IsNullOrEmpty(paymentMethod))
            {
                order.PaymentMethod = paymentMethod;
            }

            if (paymentStatus == "Completed")
            {
                order.CompletedDate = DateTime.UtcNow;
            }

            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> CompleteOrderAsync(int orderId)
        {
            return await UpdateOrderStatusAsync(orderId, "Completed");
        }
    }
}
