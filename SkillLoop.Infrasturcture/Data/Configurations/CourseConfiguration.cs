using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.Level)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Mode)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Location)
                .HasMaxLength(500);

            builder.Property(x => x.CoverUrl)
                .HasMaxLength(500);

            builder.HasIndex(x => x.TeacherId);
            builder.HasIndex(x => x.CategoryId);

            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Courses)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
