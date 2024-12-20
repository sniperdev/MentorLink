using API.Entities;

namespace API.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(User user);
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByEmailAsync(string email);
    Task UpdateUserAsync(User user);
}