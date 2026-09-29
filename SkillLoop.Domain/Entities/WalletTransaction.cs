using SkillLoop.Domain.Enums;

namespace SkillLoop.Domain.Entities
{
    public class WalletTransaction
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public WalletTransactionType Type { get; set; }
        public int Amount { get; set; }
        public int BalanceAfter { get; set; }
        public Guid? BookingId { get; set; }
        public Guid? PaymentOrderId { get; set; }
        public Guid? PromoRedemptionId { get; set; }
        public string IdempotencyKey { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Wallet Wallet { get; set; } = null!;
        public Booking? Booking { get; set; }
        public PaymentOrder? PaymentOrder { get; set; }
        public PromoRedemption? PromoRedemption { get; set; }
    }
}
