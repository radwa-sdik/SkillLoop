using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class LearningPointConfiguration : IEntityTypeConfiguration<LearningPoint>
    {
        public void Configure(EntityTypeBuilder<LearningPoint> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(x => x.CourseId);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.LearningPoints)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
