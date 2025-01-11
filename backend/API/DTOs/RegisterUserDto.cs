using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.DTOs;

public class RegisterUserDto
{
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string FullName { get; set; } = string.Empty;

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = string.Empty;

    [Required] public UserRole Role { get; set; } = UserRole.Student;
}