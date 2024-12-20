using API.Entities;
using API.Interfaces;
using API.Services;
using Moq;

namespace API.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepository.Object);
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

        await _userService.CreateUserAsync(newUser);

        _mockRepository.Verify(repo => repo.AddAsync(newUser), Times.Once());
    }

    [Fact]
    public async Task CreateUser_ShouldThrowException_WhenEmailIsAlreadyTaken()
    {
        var existingUser = new User
        {
            Id = 1,
            FullName = "Jane",
            Email = "john@example.com",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };
        var newUser = new User
        {
            Id = 2,
            FullName = "John",
            Email = "john@example.com",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetUserByEmailAsync(newUser.Email)).ReturnsAsync(existingUser);

        await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(newUser));
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
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

        var result = await _userService.GetUserByIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowException_WhenUserNotExists()
    {
        _mockRepository.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetUserByIdAsync(1));
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            FullName = "Jane",
            Email = "john@example.com",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);

        var result = await _userService.GetUserByEmailAsync(user.Email);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldThrowException_WhenEmailDoesNotExist()
    {
        _mockRepository.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetUserByEmailAsync("none@example.com"));
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

        await _userService.UpdateUserAsync(user);

        _mockRepository.Verify(repo => repo.UpdateUser(user), Times.Once());
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowInvalidOperationException_WhenUserDoesNotExist()
    {
        var user = new User
        {
            Id = 1,
            Email = "john@example.com",
            FullName = "John Doe",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync((User)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.UpdateUserAsync(user));
    }
}