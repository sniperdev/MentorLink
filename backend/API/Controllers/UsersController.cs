using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private readonly JwtTokenService _jwtTokenService;
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService, JwtTokenService jwtTokenService)
    {
        _usersService = usersService;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] RegisterUserDto registerUserDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newUser = new User
        {
            Email = registerUserDto.Email,
            FullName = registerUserDto.FullName,
            Role = registerUserDto.Role,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _usersService.CreateUserAsync(newUser, registerUserDto.Password);
            return Ok(new { Message = "User registered successfully" });
        }
        catch (ArgumentException ex)
        {
            return Conflict(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { Error = "An error occurred while registering the user", Details = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _usersService.GetUserByEmailAsync(loginDto.Email);
        if (!_usersService.ValidatePassword(user, loginDto.Password))
            return Unauthorized(new { Error = "Invalid email or password" });

        var token = _jwtTokenService.GenerateToken(user.Email, user.Role);
        return Ok(new { Token = token });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _usersService.GetUserByIdAsync(id);
            var userDto = new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
            return Ok(userDto);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            var user = await _usersService.GetUserByEmailAsync(email);
            var userDto = new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.ToString()
            };
            return Ok(userDto);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        try
        {
            await _usersService.UpdateUserAsync(id, user);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}