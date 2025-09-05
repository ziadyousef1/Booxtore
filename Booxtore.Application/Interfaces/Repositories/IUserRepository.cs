using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> UpdateAsync(ApplicationUser user);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<IEnumerable<ApplicationUser>> SearchAsync(string searchTerm);
        Task<int> GetTotalUsersAsync();
        Task<int> GetActiveUsersAsync();
        Task<int> SaveChangesAsync();
    }
}
