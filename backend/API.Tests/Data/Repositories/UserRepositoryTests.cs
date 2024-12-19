using API.Data;
using API.Data.Repositories;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Tests.Data.Repositories;

public class UserRepositoryTests
{
    private readonly DataContext _context;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser_WhenUserIsValid()
    {
        var user = new User
        {
            Id = 1,
            FullName = "John",
            Email = "john@example.com",
            Role = UserRole.Student,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        var addedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == 1);
        Assert.NotNull(addedUser);
        Assert.Equal(user, addedUser);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentNullException_WhenUserIsNull()
    {
        User user = null;

        await Assert.ThrowsAsync<ArgumentNullException>(() => _userRepository.AddAsync(user));
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            Email = "john@example.com",
            FullName = "John",
            Role = UserRole.Student,
            CreatedAt = DateTime.Now
        };

        await _userRepository.AddAsync(user);

        var findUser = await _userRepository.GetUserByIdAsync(1);

        Assert.NotNull(findUser);
        Assert.Equal(user.Id, findUser.Id);
        Assert.Equal(user.Email, findUser.Email);
    }

    [Fact]
    public async Task GetUserById_ShouldThrowException_WhenUserNotExist()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() => _userRepository.GetUserByIdAsync(1));
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            Email = "john@example.com",
            FullName = "John",
            Role = UserRole.Student,
            CreatedAt = DateTime.Now
        };

        await _userRepository.AddAsync(user);

        var findUser = await _userRepository.GetUserByEmailAsync("john@example.com");

        Assert.NotNull(findUser);
        Assert.Equal(user.Id, findUser.Id);
        Assert.Equal(user.Email, findUser.Email);
    }

    [Fact]
    public async Task GetUserByEmail_ShouldThrowException_WhenUserNotExist()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userRepository.GetUserByEmailAsync("john@example.com"));
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateUser_WhenUserExists()
    {
        var user = new User
        {
            Id = 1,
            Email = "john@example.com",
            FullName = "John",
            Role = UserRole.Student,
            CreatedAt = DateTime.Now
        };

        await _userRepository.AddAsync(user);

        user.Email = "eva@example.com";
        user.FullName = "Eva";

        await _userRepository.UpdateUser(user);

        var updatedUser = await _userRepository.GetUserByIdAsync(1);

        Assert.NotNull(updatedUser);
        Assert.Equal("eva@example.com", updatedUser.Email);
        Assert.Equal("Eva", updatedUser.FullName);
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