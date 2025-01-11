using API.Entities;
using API.Interfaces;

namespace API.Services;

public class UserService : IUserService
{
    private readonly PasswordHasherService _passwordHasherService;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, PasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) throw new InvalidOperationException("User not found");
        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null) throw new InvalidOperationException("User not found");
        return user;
    }

    public async Task UpdateUserAsync(int id, User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");

        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null) throw new InvalidOperationException("User not found");

        existingUser.Email = user.Email;
        existingUser.FullName = user.FullName;
        existingUser.Role = user.Role;
        await _userRepository.UpdateUser(existingUser);
    }

    public async Task CreateUserAsync(User user, string password)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
        if (existingUser != null)
            throw new ArgumentException("Email is already taken");

        user.PasswordHash = _passwordHasherService.HashPassword(user, password);

        await _userRepository.AddAsync(user);
    }
}