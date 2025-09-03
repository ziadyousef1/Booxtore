using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<Book> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> AddAsync(Book entity);
        Task<Book> UpdateAsync(Book entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
        Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Book>> GetByAuthorAsync(int authorId);
        Task<IEnumerable<Book>> GetFeaturedAsync(int count = 10);
        Task<IEnumerable<Book>> GetPopularAsync(int count = 10);
        Task<int> SaveChangesAsync();
    }
}
