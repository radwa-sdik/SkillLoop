namespace SkillLoop.Domain.Entities
{
    public class LearningPoint
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Text { get; set; } = null!;

        public Course Course { get; set; } = null!;
    }
}
