using API.Entities;

namespace API.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(string email, UserRole role);
}