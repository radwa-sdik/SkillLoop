namespace SkillLoop.Domain.Entities
{
    public class CourseTag
    {
        public Guid CourseId { get; set; }
        public Guid TagId { get; set; }

        public Course Course { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
