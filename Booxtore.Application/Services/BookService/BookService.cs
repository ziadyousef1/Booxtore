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

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(int authorId)
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

        public async Task<IEnumerable<Book>> GetLatestBooksAsync(int count = 10)
        {
            var allBooks = await _bookRepository.GetAllAsync();
            return allBooks.OrderByDescending(b => b.CreatedAt)
                          .Take(count)
                          .ToList();
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Book title is required.");

            if (book.Price <= 0)
                throw new ArgumentException("Book price must be greater than zero.");

            return await _bookRepository.AddAsync(book);
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
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

        
        public async Task<int> GetTotalBooksCountAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Count();
        }        public async Task<int> GetAvailableBooksCountAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Count(b => b.Status == "Available");
        }

        public async Task<int> GetBorrowedBooksCountAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Count(b => b.Status == "Borrowed");
        }

        public async Task<IEnumerable<Book>> GetBooksByStatusAsync(string status)
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Where(b => b.Status == status);
        }
    }
}
