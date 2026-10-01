namespace SkillLoop.Application.Interfaces.IService
{
    public interface ISmsService
    {
        Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
    }
}