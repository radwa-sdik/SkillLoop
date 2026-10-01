using Microsoft.AspNetCore.Http;
using SkillLoop.Application.DTOs.Auth;

namespace SkillLoop.Application.Interfaces.IService
{
    public interface IAuthService
    {
        Task<MessageResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> SocialLoginAsync(SocialLoginRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
        Task LogoutAsync(Guid userId, string refreshToken, CancellationToken cancellationToken = default);
        Task VerifyEmailAsync(VerifyEmailRequest request, CancellationToken cancellationToken = default);
        Task ResendEmailVerificationAsync(ResendEmailVerificationRequest request, CancellationToken cancellationToken = default);
        Task ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
        Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);
        Task<UserProfileResponse> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
        Task<UserProfileResponse> UploadAvatarAsync(Guid userId, IFormFile file, CancellationToken cancellationToken = default);
    }
}