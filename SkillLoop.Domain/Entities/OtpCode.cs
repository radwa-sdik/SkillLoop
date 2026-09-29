namespace SkillLoop.Domain.Entities
{
    public class OtpCode
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CodeHash { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public int Attempts { get; set; }
        public DateTime? UsedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
