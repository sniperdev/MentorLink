using API.Entities;
using API.Services;
using FluentAssertions;

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

        hashedPassword.Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public void HashPassword_NullUser_ThrowsArgumentNullException()

    {
        var password = "StrongPassword123";

        Action act = () => _passwordHasherService.HashPassword(null, password);

        act.Should().Throw<ArgumentNullException>().WithMessage("User cannot be null*");
    }


    [Fact]
    public void HashPassword_EmptyPassword_ThrowsArgumentException()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };

        Action act = () => _passwordHasherService.HashPassword(user, "");

        act.Should().Throw<ArgumentException>().WithMessage("Password cannot be null or empty*");
    }


    [Fact]
    public void VerifyPassword_ValidInputs_ReturnsTrue()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };

        var password = "StrongPassword123";

        var hashedPassword = _passwordHasherService.HashPassword(user, password);

        var isValid = _passwordHasherService.VerifyPassword(user, hashedPassword, password);

        isValid.Should().BeTrue();
    }


    [Fact]
    public void VerifyPassword_InvalidPassword_ReturnsFalse()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };
        var password = "StrongPassword123";

        var hashedPassword = _passwordHasherService.HashPassword(user, password);

        var isValid = _passwordHasherService.VerifyPassword(user, hashedPassword, "WrongPassword");

        isValid.Should().BeFalse();
    }


    [Fact]
    public void VerifyPassword_NullUser_ThrowsArgumentNullException()
    {
        var hashedPassword = "somehashedpassword";
        var providedPassword = "password";

        Assert.Throws<ArgumentNullException>(() =>
            _passwordHasherService.VerifyPassword(null, hashedPassword, providedPassword));
        Action act = () => _passwordHasherService.VerifyPassword(null, hashedPassword, providedPassword);

        act.Should().Throw<ArgumentNullException>().WithMessage("User cannot be null*");
    }


    [Fact]
    public void VerifyPassword_EmptyHashedPassword_ThrowsArgumentException()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };
        var providedPassword = "password";

        Assert.Throws<ArgumentException>(() => _passwordHasherService.VerifyPassword(user, "", providedPassword));
        Action act = () => _passwordHasherService.VerifyPassword(user, "", providedPassword);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Both hashed password and provided password must be non-empty*");
    }

    [Fact]
    public void VerifyPassword_EmptyProvidedPassword_ThrowsArgumentException()
    {
        var user = new User { Email = "test@example.com", FullName = "Test User" };
        var hashedPassword = "somehashedpassword";

        Action act = () => _passwordHasherService.VerifyPassword(user, hashedPassword, "");

        act.Should().Throw<ArgumentException>()
            .WithMessage("Both hashed password and provided password must be non-empty*");
    }
}