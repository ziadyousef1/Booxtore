using Microsoft.EntityFrameworkCore;
using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;
using Booxtore.Infrastructure.Data;

namespace Booxtore.Infrastructure.Repositories.Implementations
{
    public class BorrowingRecordRepository : IBorrowingRecordRepository
    {
        private readonly BooxtoreContext _context;

        public BorrowingRecordRepository(BooxtoreContext context)
        {
            _context = context;
        }

        public async Task<BorrowingRecord> GetByIdAsync(int id)
        {
            return await _context.BorrowingRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .FirstOrDefaultAsync(br => br.BorrowId == id);
        }

        public async Task<IEnumerable<BorrowingRecord>> GetAllAsync()
        {
            return await _context.BorrowingRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .ToListAsync();
        }

        public async Task<BorrowingRecord> AddAsync(BorrowingRecord entity)
        {
            _context.BorrowingRecords.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<BorrowingRecord> UpdateAsync(BorrowingRecord entity)
        {
            _context.BorrowingRecords.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.BorrowingRecords.FindAsync(id);
            if (entity == null) return false;

            _context.BorrowingRecords.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.BorrowingRecords.AnyAsync(br => br.BorrowId == id);
        }

        public async Task<IEnumerable<BorrowingRecord>> GetByUserIdAsync(string userId)
        {
            return await _context.BorrowingRecords
                .Include(br => br.Book)
                .ThenInclude(b => b.Author)
                .Include(br => br.Book)
                .ThenInclude(b => b.Category)
                .Where(br => br.UserId == userId)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowingRecord>> GetActiveByUserIdAsync(string userId)
        {
            return await _context.BorrowingRecords
                .Include(br => br.Book)
                .ThenInclude(b => b.Author)
                .Include(br => br.Book)
                .ThenInclude(b => b.Category)
                .Where(br => br.UserId == userId && br.Status == "Active")
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
        }

        public async Task<BorrowingRecord> GetActiveByUserAndBookAsync(string userId, int bookId)
        {
            return await _context.BorrowingRecords
                .Include(br => br.Book)
                .FirstOrDefaultAsync(br => br.UserId == userId && br.BookId == bookId && br.Status == "Active");
        }

        public async Task<IEnumerable<BorrowingRecord>> GetOverdueAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.BorrowingRecords
                .Include(br => br.Book)
                .Include(br => br.User)
                .Where(br => br.Status == "Active" && br.DueDate < today)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
