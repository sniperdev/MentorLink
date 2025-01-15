using API.Entities;

namespace API.Interfaces;

public interface IUsersRepository
{
    Task AddAsync(User user);
    Task UpdateUser(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
}