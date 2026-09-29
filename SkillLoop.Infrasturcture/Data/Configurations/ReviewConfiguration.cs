using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.EnrollmentId)
                .IsUnique();

            builder.Property(x => x.Comment)
                .HasMaxLength(2000);

            builder.HasOne(x => x.Enrollment)
                .WithOne(x => x.Review)
                .HasForeignKey<Review>(x => x.EnrollmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
