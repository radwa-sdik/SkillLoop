using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class PaymentOrderConfiguration : IEntityTypeConfiguration<PaymentOrder>
    {
        public void Configure(EntityTypeBuilder<PaymentOrder> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AmountEgp)
                .HasPrecision(18, 2);

            builder.Property(x => x.Provider)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ProviderRef)
                .HasMaxLength(255);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(x => x.ProviderRef);

            builder.HasOne(x => x.User)
                .WithMany(x => x.PaymentOrders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Package)
                .WithMany(x => x.PaymentOrders)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
