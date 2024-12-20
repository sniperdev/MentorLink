using API.Entities;

namespace API.Data.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task UpdateUser(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
}