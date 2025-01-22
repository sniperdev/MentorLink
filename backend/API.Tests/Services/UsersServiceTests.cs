using API.Entities;
using API.Interfaces;
using API.Services;
using FluentAssertions;
using Moq;

namespace API.Tests.Services;

public class UsersServiceTests
{
    private readonly Mock<IUsersRepository> _mockRepository;
    private readonly UsersService _usersService;

    public UsersServiceTests()
    {
        _mockRepository = new Mock<IUsersRepository>();
        var mockPasswordHasherService = new Mock<PasswordHasherService>();
        _usersService = new UsersService(_mockRepository.Object, mockPasswordHasherService.Object);
    }

    [Fact]
    public async Task CreateUser_ShouldCallRepositoryOnce()
    {
        var newUser = new User
        {
            Id = 1,
            Email = "example@example.com",
            FullName = "John Doe",
            Role = UserRole.Student,
            CreatedAt = DateTime.Now
        };

        _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<User>())).Verifiable();

        await _usersService.CreateUserAsync(newUser, "password123");

        _mockRepository.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Email == newUser.Email)), Times.Once());
    }

    [Fact]
    public async Task CreateUser_ShouldThrowException_WhenEmailIsAlreadyTaken()
    {
        var existingUser = new User { Email = "john@example.com" };
        var newUser = new User { Email = "john@example.com" };

        _mockRepository.Setup(repo => repo.GetUserByEmailAsync(newUser.Email)).ReturnsAsync(existingUser);

        var act = () => _usersService.CreateUserAsync(newUser, "password123");

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Email is already taken*");
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            Email = "john@example.com",
            FullName = "Jane Doe",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

        var result = await _usersService.GetUserByIdAsync(user.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowException_WhenUserNotExists()
    {
        _mockRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

        Func<Task> act = () => _usersService.GetUserByIdAsync(1);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("User not found");
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            Email = "john@example.com",
            FullName = "Jane Doe",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

        var result = await _usersService.GetUserByEmailAsync(user.Email);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldThrowException_WhenEmailDoesNotExist()
    {
        _mockRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

        Func<Task> act = () => _usersService.GetUserByEmailAsync("none@example.com");

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("User not found");
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            FullName = "Jane",
            Email = "john@example.com",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        _mockRepository.Setup(repo => repo.UpdateUser(user)).Verifiable();

        user.Email = "updated@example.com";
        user.FullName = "Updated name";
        await _usersService.UpdateUserAsync(user.Id, user);

        _mockRepository.Verify(
            repo => repo.UpdateUser(It.Is<User>(u => u.Email == "updated@example.com" && u.FullName == "Updated name")),
            Times.Once());
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowInvalidOperationException_WhenUserDoesNotExist()
    {
        var user = new User
        {
            Id = 99,
            Email = "john@example.com",
            FullName = "John Doe"
        };

        _mockRepository.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync((User)null);

        var act = () => _usersService.UpdateUserAsync(99, user);

        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("User not found");
    }
}