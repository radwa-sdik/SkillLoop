namespace SkillLoop.Application.Interfaces.IService
{
    public interface IOtpService
    {
        Task SendEmailVerificationAsync(Guid userId, string email, CancellationToken cancellationToken = default);
        Task SendPasswordResetAsync(Guid userId, string phoneNumber, CancellationToken cancellationToken = default);
        Task VerifyAsync(Guid userId, string code, string purpose, CancellationToken cancellationToken = default);
    }
}