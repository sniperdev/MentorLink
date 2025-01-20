using API.Entities;

public interface IJwtTokenService
{
    string GenerateToken(string email, UserRole role);
}