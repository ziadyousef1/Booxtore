using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;
using Booxtore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booxtore.Infrastructure.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly BooxtoreContext _context;
        private readonly DbSet<Book> _dbSet;

        public BookRepository(BooxtoreContext context)
        {
            _context = context;
            _dbSet = context.Set<Book>();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _dbSet
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<Book> AddAsync(Book entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Book> UpdateAsync(Book entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }

        public async Task<IEnumerable<Book>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _dbSet
                .Where(b => b.Title.Contains(searchTerm) || 
                           (b.Author != null && b.Author.Name.Contains(searchTerm)) ||
                           (b.Description != null && b.Description.Contains(searchTerm)))
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(b => b.CategoryId == categoryId)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByAuthorAsync(int authorId)
        {
            return await _dbSet
                .Where(b => b.AuthorId == authorId)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetFeaturedAsync(int count = 10)
        {
            return await _dbSet
                .Where(b => b.IsAvailableForPurchase == true)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetPopularAsync(int count = 10)
        {
            return await _dbSet
                .OrderByDescending(b => b.Price)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
