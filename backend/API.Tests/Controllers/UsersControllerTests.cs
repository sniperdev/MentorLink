using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _controller;
    private readonly Mock<IUsersService> _userServiceMock;
    private readonly Mock<JwtTokenService> _jwtServiceMock = new();

    public UsersControllerTests()
    {
        _userServiceMock = new Mock<IUsersService>();
        _controller = new UsersController(_userServiceMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task CreateUser_ValidUser_ReturnsOk()
    {
        var registerUserDto = new RegisterUserDto
        {
            Email = "test@example.com",
            FullName = "Test User",
            Password = "StrongPassword123"
        };

        _userServiceMock
            .Setup(service => service.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.CreateUser(registerUserDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateUser_DuplicateUser_ReturnsConflict()
    {
        var registerUserDto = new RegisterUserDto
        {
            Email = "duplicate@example.com",
            FullName = "Duplicate User",
            Password = "Password123"
        };

        _userServiceMock
            .Setup(service => service.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
            .Throws(new ArgumentException("User already exists"));

        var result = await _controller.CreateUser(registerUserDto);

        var conflictResult = Assert.IsType<ConflictObjectResult>(result);
        Assert.Equal(409, conflictResult.StatusCode);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserExists()
    {
        var user = new User { Id = 1, Email = "test@example.com", FullName = "Test User" };
        _userServiceMock.Setup(service => service.GetUserByIdAsync(user.Id))
            .ReturnsAsync(user);

        var result = await _controller.GetUserById(user.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(user, okResult.Value);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        _userServiceMock.Setup(service => service.GetUserByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException("User not found"));

        var result = await _controller.GetUserById(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found", notFoundResult.Value?.ToString());
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnNoContent_WhenUserIsUpdated()
    {
        var user = new User { Id = 1, Email = "test@example.com", FullName = "Updated User" };
        _userServiceMock.Setup(service => service.UpdateUserAsync(1, user))
            .Returns(Task.CompletedTask);

        var result = await _controller.UpdateUser(user.Id, user);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var user = new User { Id = 1, Email = "test@example.com" };
        _userServiceMock.Setup(service => service.UpdateUserAsync(99, It.IsAny<User>()))
            .ThrowsAsync(new InvalidOperationException("User not found"));

        var result = await _controller.UpdateUser(99, user);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found", notFoundResult.Value);
    }
}