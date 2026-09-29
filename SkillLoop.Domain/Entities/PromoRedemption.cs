namespace SkillLoop.Domain.Entities
{
    public class PromoRedemption
    {
        public Guid Id { get; set; }
        public Guid PromoCodeId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RedeemedAt { get; set; }

        public PromoCode PromoCode { get; set; } = null!;
        public User User { get; set; } = null!;

        public ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}
