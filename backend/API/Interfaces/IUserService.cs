using API.Entities;

namespace API.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(User user, string password);
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByEmailAsync(string email);
    Task UpdateUserAsync(int id, User user);
}