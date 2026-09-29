namespace SkillLoop.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? IconUrl { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
