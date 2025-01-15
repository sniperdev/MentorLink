using API.Entities;
using API.Interfaces;

namespace API.Services;

public class UsersService : IUsersService
{
    private readonly PasswordHasherService _passwordHasherService;
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository, PasswordHasherService passwordHasherService)
    {
        _usersRepository = usersRepository;
        _passwordHasherService = passwordHasherService;
    }

    public bool ValidatePassword(User user, string password)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");
        if(string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Password cannot be null or empty", nameof(password));

        return _passwordHasherService.VerifyPassword(user, user.PasswordHash, password);
    }
    
    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _usersRepository.GetUserByIdAsync(id);
        if (user == null) throw new InvalidOperationException("User not found");
        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _usersRepository.GetUserByEmailAsync(email);
        if (user == null) throw new InvalidOperationException("User not found");
        return user;
    }

    public async Task UpdateUserAsync(int id, User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");

        var existingUser = await _usersRepository.GetUserByIdAsync(id);
        if (existingUser == null) throw new InvalidOperationException("User not found");

        existingUser.Email = user.Email;
        existingUser.FullName = user.FullName;
        existingUser.Role = user.Role;
        await _usersRepository.UpdateUser(existingUser);
    }

    public async Task CreateUserAsync(User user, string password)
    {
        var existingUser = await _usersRepository.GetUserByEmailAsync(user.Email);
        if (existingUser != null)
            throw new ArgumentException("Email is already taken");

        user.PasswordHash = _passwordHasherService.HashPassword(user, password);

        await _usersRepository.AddAsync(user);
    }
}