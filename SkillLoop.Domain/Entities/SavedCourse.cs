namespace SkillLoop.Domain.Entities
{
    public class SavedCourse
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }

        public User User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
