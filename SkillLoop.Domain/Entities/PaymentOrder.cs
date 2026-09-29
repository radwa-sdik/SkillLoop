using SkillLoop.Domain.Enums;

namespace SkillLoop.Domain.Entities
{
    public class PaymentOrder
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public decimal AmountEgp { get; set; }
        public string Provider { get; set; } = null!;
        public string? ProviderRef { get; set; }
        public PaymentStatus Status { get; set; }

        public User User { get; set; } = null!;
        public CreditPackage Package { get; set; } = null!;

        public ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}
