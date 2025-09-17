using Booxtore.Application.Interfaces.Repositories;
using Booxtore.Application.Interfaces.Services;
using Booxtore.Domain.Models;

namespace Booxtore.Application.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string searchTerm)
    {
        return await _userRepository.SearchAsync(searchTerm);
    }

    public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
    {
        if (string.IsNullOrWhiteSpace(user.FirstName))
            throw new ArgumentException("First name is required.");

        if (string.IsNullOrWhiteSpace(user.LastName))
            throw new ArgumentException("Last name is required.");

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new ArgumentException("Email is required.");

        user.UpdatedAt = DateTime.UtcNow;
        return await _userRepository.UpdateAsync(user);
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return false;

        return await _userRepository.DeleteAsync(id);
    }

    public async Task<bool> ToggleUserStatusAsync(string id, string status)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return false;

        user.Status = status;
        user.UpdatedAt = DateTime.UtcNow;
        
        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<int> GetTotalUsersAsync()
    {
        return await _userRepository.GetTotalUsersAsync();
    }

    public async Task<int> GetActiveUsersAsync()
    {
        return await _userRepository.GetActiveUsersAsync();
    }

    public async Task<int> GetInactiveUsersAsync()
    {
        var total = await GetTotalUsersAsync();
        var active = await GetActiveUsersAsync();
        return total - active;
    }

    public async Task<int> GetNewUsersThisMonthAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var thisMonth = DateTime.UtcNow.Month;
        var thisYear = DateTime.UtcNow.Year;
        
        return users.Count(u => u.CreatedAt?.Month == thisMonth && u.CreatedAt?.Year == thisYear);
    }
}
