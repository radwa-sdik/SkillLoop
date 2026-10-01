namespace SkillLoop.Application.DTOs.Auth
{
    public record RegisterRequest(string FullName, string Email, string Password, string ConfirmPassword);
    public record LoginRequest(string Email, string Password);
    public record SocialLoginRequest(string Provider, string IdToken, string? PhoneNumber = null);
    public record RefreshTokenRequest(string RefreshToken);
    public record VerifyEmailRequest(string Email, string Code);
    public record ResendEmailVerificationRequest(string Email);
    public record ForgotPasswordRequest(string PhoneNumber);
    public record ResetPasswordRequest(string PhoneNumber, string Code, string NewPassword, string ConfirmPassword);
    public record UpdateProfileRequest(string FullName, string? Headline, string? City, string? PhoneNumber);
    public record MessageResponse(string Message);
    public record AuthResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAt, UserProfileResponse User);
    public record UserProfileResponse(Guid Id, string FullName, string Email, string? PhoneNumber, string? AvatarUrl, string? Headline, string? City, bool EmailVerified, DateTime CreatedAt);
}