using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Repositories
{
    public interface IBorrowingRecordRepository
    {
        Task<BorrowingRecord> GetByIdAsync(int id);
        Task<IEnumerable<BorrowingRecord>> GetAllAsync();
        Task<BorrowingRecord> AddAsync(BorrowingRecord entity);
        Task<BorrowingRecord> UpdateAsync(BorrowingRecord entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<BorrowingRecord>> GetByUserIdAsync(string userId);
        Task<IEnumerable<BorrowingRecord>> GetActiveByUserIdAsync(string userId);
        Task<BorrowingRecord> GetActiveByUserAndBookAsync(string userId, int bookId);
        Task<IEnumerable<BorrowingRecord>> GetOverdueAsync();
        Task<int> SaveChangesAsync();
    }
}
