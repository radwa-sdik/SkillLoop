using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.LearnerId, x.CourseId })
                .IsUnique();

            builder.HasOne(x => x.Learner)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
}
}
