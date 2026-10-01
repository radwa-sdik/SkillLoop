namespace SkillLoop.Application.Interfaces.IService
{
    public interface IEmailService
    {
        Task SendAsync(string email, string subject, string body, CancellationToken cancellationToken = default);
    }
}