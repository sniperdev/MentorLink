using API.Data;
using API.Data.Repositories;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Tests.Data.Repositories;

public class UserRepositoryTests : IAsyncLifetime
{
    private DataContext _context;
    private UserRepository _userRepository;

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _userRepository = new UserRepository(_context);

        await _context.Users.AddRangeAsync(
            new User
            {
                Id = 1, FullName = "John Doe", Email = "john@example.com", Role = UserRole.Student,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Id = 2, FullName = "Jane Doe", Email = "jane@example.com", Role = UserRole.Student,
                CreatedAt = DateTime.UtcNow
            });
        await _context.SaveChangesAsync();
    }

    public Task DisposeAsync()
    {
        _context.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser_WhenUserIsValid()
    {
        var user = new User
        {
            Id = 3,
            FullName = "John",
            Email = "john@example.com",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        var addedUser = await _context.Users.FindAsync(3);
        Assert.NotNull(addedUser);
        Assert.Equal(user.Email, addedUser.Email);
        Assert.Equal(user.FullName, addedUser.FullName);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentNullException_WhenUserIsNull()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _userRepository.AddAsync(null));
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserExists()
    {
        var user = await _context.Users.FindAsync(1);

        Assert.NotNull(user);
        Assert.Equal(1, user.Id);
        Assert.Equal("john@example.com", user.Email);
    }

    [Fact]
    public async Task GetUserById_ShouldThrowException_WhenUserNotExist()
    {
        var result = await _userRepository.GetUserByIdAsync(99);
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
    {
        var user = await _userRepository.GetUserByEmailAsync("john@example.com");

        Assert.NotNull(user);
        Assert.Equal(1, user.Id);
        Assert.Equal("john@example.com", user.Email);
    }

    [Fact]
    public async Task GetUserByEmail_ShouldThrowException_WhenUserNotExist()
    {
        var result = await _userRepository.GetUserByEmailAsync("other@example.com");
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateUser_WhenUserExists()
    {
        var user = await _userRepository.GetUserByIdAsync(1);
        user.FullName = "Updated Name";
        user.Email = "updated@example.com";

        await _userRepository.UpdateUser(user);

        var updatedUser = await _userRepository.GetUserByIdAsync(1);

        Assert.NotNull(updatedUser);
        Assert.Equal("updated@example.com", updatedUser.Email);
        Assert.Equal("Updated Name", updatedUser.FullName);
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowInvalidOperationException_WhenUserNotExist()
    {
        var user = new User
        {
            Id = 1,
            Email = "john@example.com",
            FullName = "John",
            Role = UserRole.Student,
            CreatedAt = DateTime.Now
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _userRepository.UpdateUser(user));
    }
}