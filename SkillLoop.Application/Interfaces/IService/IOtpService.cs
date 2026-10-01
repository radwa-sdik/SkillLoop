namespace SkillLoop.Application.Interfaces.IService
{
    public interface IOtpService
    {
        Task SendAsync(Guid userId, string email, string purpose, CancellationToken cancellationToken = default);
        Task VerifyAsync(Guid userId, string code, string purpose, CancellationToken cancellationToken = default);
    }
}