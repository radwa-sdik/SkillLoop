namespace SkillLoop.Application.Interfaces.IService
{
    public record SocialIdentity(string Provider, string ProviderKey, string Email, string FullName);
    public interface ISocialLoginService
    {
        Task<SocialIdentity> ValidateAsync(string provider, string token, CancellationToken cancellationToken = default);
    }
}
