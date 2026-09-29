namespace SkillLoop.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
