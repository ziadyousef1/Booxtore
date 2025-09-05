using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync();
        Task AddToCartAsync(int bookId, int quantity = 1);
        Task RemoveFromCartAsync(int bookId);
        Task UpdateQuantityAsync(int bookId, int quantity);
        Task ClearCartAsync();
        Task<int> GetCartItemCountAsync();
    }
}
