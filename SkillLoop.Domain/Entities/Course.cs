using SkillLoop.Domain.Enums;

namespace SkillLoop.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public Guid TeacherId { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Level { get; set; } = null!;
        public int PricePerSession { get; set; }
        public int DurationMinutes { get; set; }
        public int TotalSessions { get; set; }
        public CourseMode Mode { get; set; }
        public string? Location { get; set; }
        public string? CoverUrl { get; set; }
        public bool IsActive { get; set; }

        public User Teacher { get; set; } = null!;
        public Category Category { get; set; } = null!;

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();

        public ICollection<LearningPoint> LearningPoints { get; set; } = new List<LearningPoint>();
        public ICollection<SavedCourse> SavedCourses { get; set; } = new List<SavedCourse>();
        public ICollection<Slot> Slots { get; set; } = new List<Slot>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}
