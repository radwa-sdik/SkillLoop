namespace SkillLoop.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid EnrollmentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public Enrollment Enrollment { get; set; } = null!;
    }
}
