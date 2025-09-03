using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<IEnumerable<Book>> GetByAuthorAsync(int authorId);
        Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10);
        Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 10);
        Task<Book> CreateBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);
    }
}
