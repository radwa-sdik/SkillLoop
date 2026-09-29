namespace SkillLoop.Domain.Entities
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid LearnerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Course Course { get; set; } = null!;
        public User Learner { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public Review? Review { get; set; }
    }
}
