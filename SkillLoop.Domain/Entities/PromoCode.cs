namespace SkillLoop.Domain.Entities
{
    public class PromoCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public int Credits { get; set; }
        public int MaxUses { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; }

        public ICollection<PromoRedemption> Redemptions { get; set; } = new List<PromoRedemption>();
    }
}
