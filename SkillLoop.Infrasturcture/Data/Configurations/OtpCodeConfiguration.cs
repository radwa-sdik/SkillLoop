using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
    {
        public void Configure(EntityTypeBuilder<OtpCode> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CodeHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Purpose)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => new { x.UserId, x.Purpose });

            builder.HasOne(x => x.User)
                .WithMany(x => x.OtpCodes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
