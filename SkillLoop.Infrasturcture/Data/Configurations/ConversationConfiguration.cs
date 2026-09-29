using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.TeacherId, x.LearnerId, x.CourseId })
                .IsUnique();

            builder.HasOne(x => x.Course)
                .WithMany(x => x.Conversations)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Teacher)
                .WithMany(x => x.TeachingConversations)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Learner)
                .WithMany(x => x.LearningConversations)
                .HasForeignKey(x => x.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
}
}
