using API.Controllers;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _controller;
    private readonly Mock<IJwtTokenService> _jwtServiceMock;
    private readonly Mock<IUsersService> _userServiceMock;

    public UsersControllerTests()
    {
        _userServiceMock = new Mock<IUsersService>();
        _jwtServiceMock = new Mock<IJwtTokenService>();
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

        result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
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

        result.Should().BeOfType<ConflictObjectResult>().Which.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task Login_ReturnsOkWithToken_WhenCredentialsAreValid()
    {
        var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
        var user = new User { Email = "test@example.com", Role = UserRole.Student };
        var token = "token";
        _userServiceMock.Setup(s => s.GetUserByEmailAsync(loginDto.Email)).ReturnsAsync(user);
        _userServiceMock.Setup(s => s.ValidatePassword(user, loginDto.Password)).Returns(true);
        _jwtServiceMock.Setup(s => s.GenerateToken(user.Email, user.Role)).Returns(token);

        var result = await _controller.Login(loginDto);

        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(new { Token = token });
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var loginDto = new LoginDto { Email = "test@example.com", Password = "WrongPassword123" };
        var user = new User { Email = loginDto.Email, Role = UserRole.Student };

        _userServiceMock.Setup(service => service.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);
        _userServiceMock.Setup(service => service.ValidatePassword(user, loginDto.Password))
            .Returns(false);

        var result = await _controller.Login(loginDto);

        result.Should().BeOfType<UnauthorizedObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { Error = "Invalid email or password" });
    }

    [Fact]
    public async Task Login_InvalidEmail_ReturnsUnauthorized()
    {
        var loginDto = new LoginDto { Email = "nonexistent@example.com", Password = "SomePassword123" };

        _userServiceMock.Setup(service => service.GetUserByEmailAsync(loginDto.Email))
            .ReturnsAsync((User)null);

        var result = await _controller.Login(loginDto);

        result.Should().BeOfType<UnauthorizedObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { Error = "Invalid email or password" });
    }

    [Fact]
    public async Task Login_InvalidModelState_ReturnsBadRequest()
    {
        var loginDto = new LoginDto();
        _controller.ModelState.AddModelError("Email", "Email is required.");

        var result = await _controller.Login(loginDto);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeOfType<SerializableError>()
            .Which.Should().ContainKey("Email")
            .WhoseValue.Should().BeEquivalentTo(new[] { "Email is required." });
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserExists()
    {
        var user = new User { Id = 1, Email = "test@example.com", FullName = "Test User" };
        var userDto = new UserDto { Email = user.Email, FullName = user.FullName, Role = user.Role.ToString() };
        _userServiceMock
            .Setup(service =>
                service.GetUserByIdAsync(user.Id))
            .ReturnsAsync(user);

        var result = await _controller.GetUserById(user.Id);

        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        _userServiceMock.Setup(service => service.GetUserByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new InvalidOperationException("User not found"));

        var result = await _controller.GetUserById(1);

        result.Should().BeOfType<NotFoundObjectResult>().Which.Value.Should().Be("User not found");
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnNoContent_WhenUserIsUpdated()
    {
        var user = new User { Id = 1, Email = "test@example.com", FullName = "Updated User" };
        _userServiceMock.Setup(service => service.UpdateUserAsync(1, user))
            .Returns(Task.CompletedTask);

        var result = await _controller.UpdateUser(user.Id, user);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        var user = new User { Id = 1, Email = "test@example.com" };
        _userServiceMock.Setup(service => service.UpdateUserAsync(99, It.IsAny<User>()))
            .ThrowsAsync(new InvalidOperationException("User not found"));

        var result = await _controller.UpdateUser(99, user);

        result.Should().BeOfType<NotFoundObjectResult>().Which.Value.Should().Be("User not found");
    }
}