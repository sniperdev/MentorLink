using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key] public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(255)] public string PasswordHash { get; set; } = string.Empty;

    [Required] [MaxLength(100)] public string FullName { get; set; } = string.Empty;

    [Required] public UserRole Role { get; set; } = UserRole.Student;

    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum UserRole
{
    Admin = 0,
    Mentor = 1,
    Student = 2
}