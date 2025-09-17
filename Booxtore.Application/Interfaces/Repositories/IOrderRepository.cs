using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<PurchaseOrder> GetByIdAsync(int id);
        Task<IEnumerable<PurchaseOrder>> GetByUserIdAsync(string userId);
        Task<PurchaseOrder> AddAsync(PurchaseOrder order);
        Task<PurchaseOrder> UpdateAsync(PurchaseOrder order);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<PurchaseOrder>> GetAllAsync();
    }
}
