using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Services;

public class PasswordHasherService
{
    private readonly PasswordHasher<User> _passwordHasher;

    public PasswordHasherService()
    {
        _passwordHasher = new PasswordHasher<User>();
    }

    public string HashPassword(User user, string password)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user), "User cannot be null");

        if (string.IsNullOrWhiteSpace(hashedPassword) || string.IsNullOrWhiteSpace(providedPassword))
            throw new ArgumentException("Both hashed password and provided password must be non-empty");

        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}