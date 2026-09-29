using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.IdempotencyKey)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(x => x.IdempotencyKey)
                .IsUnique();

            builder.HasIndex(x => x.WalletId);

            builder.HasOne(x => x.Wallet)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.WalletTransactions)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaymentOrder)
                .WithMany(x => x.WalletTransactions)
                .HasForeignKey(x => x.PaymentOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PromoRedemption)
                .WithMany(x => x.WalletTransactions)
                .HasForeignKey(x => x.PromoRedemptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
}
}
