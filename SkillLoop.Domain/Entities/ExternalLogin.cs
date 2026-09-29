namespace SkillLoop.Domain.Entities
{
    public class ExternalLogin
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Provider { get; set; } = null!;
        public string ProviderKey { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
