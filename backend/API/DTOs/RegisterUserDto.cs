using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using API.Entities;

namespace API.DTOs;

public class RegisterUserDto
{
    [JsonPropertyName("email")]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("fullName")]
    [Required(ErrorMessage = "Full name is required.")]
    [MaxLength(100, ErrorMessage = "Full name must not exceed 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;


    [JsonPropertyName("role")]
    [Required(ErrorMessage = "Role is required.")]
    public UserRole Role { get; set; } = UserRole.Student;
}