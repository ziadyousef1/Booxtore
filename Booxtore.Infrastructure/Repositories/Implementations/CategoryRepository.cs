using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;
using Booxtore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booxtore.Infrastructure.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BooxtoreContext _context;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository(BooxtoreContext context)
        {
            _context = context;
            _dbSet = context.Set<Category>();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Category> AddAsync(Category entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Category> UpdateAsync(Category entity)
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

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithBooksAsync()
        {
            return await _dbSet
                .Include(c => c.Books)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
