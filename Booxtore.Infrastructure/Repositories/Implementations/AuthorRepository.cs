using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;
using Booxtore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booxtore.Infrastructure.Repositories.Implementations
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BooxtoreContext _context;

        public AuthorRepository(BooxtoreContext context)
        {
            _context = context;
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<Author> AddAsync(Author entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            _context.Authors.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Author> UpdateAsync(Author entity)
        {
            _context.Authors.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Authors.AnyAsync(a => a.AuthorId == id);
        }

        public async Task<IEnumerable<Author>> SearchAsync(string searchTerm)
        {
            return await _context.Authors
                .Where(a => a.Name.Contains(searchTerm) || 
                           (a.Biography != null && a.Biography.Contains(searchTerm)))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
