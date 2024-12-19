using API.Data.Interfaces;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");

        await _context.Users.AddAsync(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error saving user to the database", ex);
        }
    }

    public async Task UpdateUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");

        _context.Users.Update(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error updating user in the database", ex);
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new InvalidOperationException("User not found");
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        if (user == null) throw new InvalidOperationException("User not found");
        return user;
    }
}