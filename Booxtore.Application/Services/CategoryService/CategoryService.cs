using Booxtore.Application.Interfaces.Services;
using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;

namespace Booxtore.Application.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;

        public CategoryService(ICategoryRepository categoryRepository, IBookRepository bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _categoryRepository.GetByNameAsync(name);
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithBooksAsync()
        {
            return await _categoryRepository.GetCategoriesWithBooksAsync();
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name is required.");

            var existingCategory = await _categoryRepository.GetByNameAsync(category.Name);
            if (existingCategory != null)
                throw new InvalidOperationException("Category with this name already exists.");

            return await _categoryRepository.AddAsync(category);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(category.CategoryId);
            if (existingCategory == null)
                throw new ArgumentException("Category not found.");

            return await _categoryRepository.UpdateAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var booksInCategory = await _bookRepository.GetByCategoryAsync(id);
            if (booksInCategory.Any())
                throw new InvalidOperationException("Cannot delete category that contains books.");

            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _bookRepository.GetByCategoryAsync(categoryId);
        }
    }
}
