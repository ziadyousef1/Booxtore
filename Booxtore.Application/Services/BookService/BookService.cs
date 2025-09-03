using Booxtore.Application.Interfaces.Services;
using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;

namespace Booxtore.Application.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            return await _bookRepository.SearchAsync(searchTerm);
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _bookRepository.GetByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Book>> GetByAuthorAsync(int authorId)
        {
            return await _bookRepository.GetByAuthorAsync(authorId);
        }

        public async Task<IEnumerable<Book>> GetFeaturedBooksAsync(int count = 10)
        {
            return await _bookRepository.GetFeaturedAsync(count);
        }

        public async Task<IEnumerable<Book>> GetPopularBooksAsync(int count = 10)
        {
            return await _bookRepository.GetPopularAsync(count);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            // Add business logic validation here
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title is required.");

            if (book.Price <= 0)
                throw new ArgumentException("Book price must be greater than zero.");

            return await _bookRepository.AddAsync(book);
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            // Add business logic validation here
            var existingBook = await _bookRepository.GetByIdAsync(book.BookId);
            if (existingBook == null)
                throw new ArgumentException("Book not found.");

            return await _bookRepository.UpdateAsync(book);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);
            if (existingBook == null)
                return false;

            return await _bookRepository.DeleteAsync(id);
        }
    }
}
