using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class PromoRedemptionConfiguration : IEntityTypeConfiguration<PromoRedemption>
    {
        public void Configure(EntityTypeBuilder<PromoRedemption> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.PromoCodeId, x.UserId })
                .IsUnique();

            builder.HasOne(x => x.PromoCode)
                .WithMany(x => x.Redemptions)
                .HasForeignKey(x => x.PromoCodeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany(x => x.PromoRedemptions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
