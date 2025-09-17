using Booxtore.Application.Interfaces.Services;
using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace Booxtore.Application.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBookRepository _bookRepository;
        private const string CartSessionKey = "ShoppingCart";

        public CartService(IHttpContextAccessor httpContextAccessor, IBookRepository bookRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _bookRepository = bookRepository;
        }

        public async Task<Cart> GetCartAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null) return new Cart();

            var cartJson = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new Cart();
            }

            var cart = JsonSerializer.Deserialize<Cart>(cartJson) ?? new Cart();
            
            foreach (var item in cart.Items)
            {
                var book = await _bookRepository.GetByIdAsync(item.BookId);
                if (book != null)
                {
                    item.Book = new Book
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        CoverImageUrl = book.CoverImageUrl,
                        Price = book.Price,
                        IsAvailableForPurchase = book.IsAvailableForPurchase
                    };
                    
                    item.Title = book.Title;
                    item.CoverImageUrl = book.CoverImageUrl ?? "";
                    item.Price = (decimal)book.Price;
                }
            }

            return cart;
        }

        public async Task AddToCartAsync(int bookId, int quantity = 1)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null || !book.IsAvailableForPurchase.GetValueOrDefault())
                throw new InvalidOperationException("Book is not available for purchase");

            var cart = await GetCartAsync();
            
            var cleanBook = new Book
            {
                BookId = book.BookId,
                Title = book.Title,
                CoverImageUrl = book.CoverImageUrl,
                Price = book.Price,
                IsAvailableForPurchase = book.IsAvailableForPurchase
            };
            
            cart.AddItem(cleanBook, quantity);
            await SaveCartAsync(cart);
        }

        public async Task RemoveFromCartAsync(int bookId)
        {
            var cart = await GetCartAsync();
            cart.RemoveItem(bookId);
            await SaveCartAsync(cart);
        }

        public async Task UpdateQuantityAsync(int bookId, int quantity)
        {
            var cart = await GetCartAsync();
            cart.UpdateQuantity(bookId, quantity);
            await SaveCartAsync(cart);
        }

        public async Task ClearCartAsync()
        {
            var cart = new Cart();
            await SaveCartAsync(cart);
        }

        public async Task<int> GetCartItemCountAsync()
        {
            var cart = await GetCartAsync();
            return cart.TotalItems;
        }

        private async Task SaveCartAsync(Cart cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var cleanCart = new Cart
                {
                    Items = cart.Items.Select(item => new CartItem
                    {
                        BookId = item.BookId,
                        Title = item.Title,
                        CoverImageUrl = item.CoverImageUrl,
                        Price = item.Price,
                        Quantity = item.Quantity
                    }).ToList()
                };
                
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = false
                };
                
                var cartJson = JsonSerializer.Serialize(cleanCart, options);
                session.SetString(CartSessionKey, cartJson);
            }
            await Task.CompletedTask;
        }
    }
}
