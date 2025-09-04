using Booxtore.Domain.Models;

namespace Booxtore.Application.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
    Task<ApplicationUser?> GetUserByIdAsync(string id);
    Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm);
    Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
    Task<bool> ToggleUserStatusAsync(string id, string status);
    Task<bool> DeleteUserAsync(string id);
    Task<int> GetTotalUsersAsync();
    Task<int> GetActiveUsersAsync();
    Task<int> GetInactiveUsersAsync();
    Task<int> GetNewUsersThisMonthAsync();
}
