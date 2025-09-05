using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;
using Booxtore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booxtore.Infrastructure.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BooxtoreContext _context;

        public OrderRepository(BooxtoreContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrder> GetByIdAsync(int id)
        {
            return await _context.PurchaseOrders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<PurchaseOrder>> GetByUserIdAsync(string userId)
        {
            return await _context.PurchaseOrders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<PurchaseOrder> AddAsync(PurchaseOrder order)
        {
            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<PurchaseOrder> UpdateAsync(PurchaseOrder order)
        {
            _context.PurchaseOrders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.PurchaseOrders.FindAsync(id);
            if (order == null) return false;

            _context.PurchaseOrders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllAsync()
        {
            return await _context.PurchaseOrders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
