using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class SavedCourseConfiguration : IEntityTypeConfiguration<SavedCourse>
    {
        public void Configure(EntityTypeBuilder<SavedCourse> builder)
        {
            builder.HasKey(x => new { x.UserId, x.CourseId });

            builder.HasOne(x => x.User)
                .WithMany(x => x.SavedCourses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.SavedCourses)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
