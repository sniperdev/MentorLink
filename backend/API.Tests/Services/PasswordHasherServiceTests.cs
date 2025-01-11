using API.Entities;
using API.Services;

namespace API.Tests.Services;

public class PasswordHasherServiceTests
{
    private readonly PasswordHasherService _passwordHasherService;

    public PasswordHasherServiceTests()
    {
        _passwordHasherService = new PasswordHasherService();
    }

    [Fact]
    public void HashPassword_ValidUserAndPassword_ReturnsHashedPassword()

    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };

        var password = "StrongPassword123";

        var hashedPassword = _passwordHasherService.HashPassword(user, password);

        Assert.False(string.IsNullOrWhiteSpace(hashedPassword));
    }


    [Fact]
    public void HashPassword_NullUser_ThrowsArgumentNullException()

    {
        var password = "StrongPassword123";

        Assert.Throws<ArgumentNullException>(() => _passwordHasherService.HashPassword(null, password));
    }


    [Fact]
    public void HashPassword_EmptyPassword_ThrowsArgumentException()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };

        Assert.Throws<ArgumentException>(() => _passwordHasherService.HashPassword(user, ""));
    }


    [Fact]
    public void VerifyPassword_ValidInputs_ReturnsTrue()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };

        var password = "StrongPassword123";

        var hashedPassword = _passwordHasherService.HashPassword(user, password);

        var isValid = _passwordHasherService.VerifyPassword(user, hashedPassword, password);

        Assert.True(isValid);
    }


    [Fact]
    public void VerifyPassword_InvalidPassword_ReturnsFalse()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };
        var password = "StrongPassword123";

        var hashedPassword = _passwordHasherService.HashPassword(user, password);

        var isValid = _passwordHasherService.VerifyPassword(user, hashedPassword, "WrongPassword");

        Assert.False(isValid);
    }


    [Fact]
    public void VerifyPassword_NullUser_ThrowsArgumentNullException()
    {
        var hashedPassword = "somehashedpassword";
        var providedPassword = "password";

        Assert.Throws<ArgumentNullException>(() =>
            _passwordHasherService.VerifyPassword(null, hashedPassword, providedPassword));
    }


    [Fact]
    public void VerifyPassword_EmptyHashedPassword_ThrowsArgumentException()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };
        var providedPassword = "password";

        Assert.Throws<ArgumentException>(() => _passwordHasherService.VerifyPassword(user, "", providedPassword));
    }


    [Fact]
    public void VerifyPassword_EmptyProvidedPassword_ThrowsArgumentException()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };
        var hashedPassword = "somehashedpassword";

        Assert.Throws<ArgumentException>(() => _passwordHasherService.VerifyPassword(user, hashedPassword, ""));
    }
}