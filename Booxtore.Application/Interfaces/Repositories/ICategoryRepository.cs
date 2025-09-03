using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> AddAsync(Category entity);
        Task<Category> UpdateAsync(Category entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<Category> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetCategoriesWithBooksAsync();
        Task<int> SaveChangesAsync();
    }
}
