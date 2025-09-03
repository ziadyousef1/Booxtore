using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetCategoriesWithBooksAsync();
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
    }
}
