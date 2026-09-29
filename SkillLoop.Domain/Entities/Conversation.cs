namespace SkillLoop.Domain.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid LearnerId { get; set; }
        public DateTime LastMessageAt { get; set; }

        public Course Course { get; set; } = null!;
        public User Teacher { get; set; } = null!;
        public User Learner { get; set; } = null!;

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
