using SkillLoop.Domain.Entities;

namespace SkillLoop.Application.Interfaces.IService
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
        string CreateRefreshToken();
        string HashToken(string token);
        bool VerifyToken(string token, string hash);
        DateTime AccessTokenExpiresAt { get; }
    }
}
