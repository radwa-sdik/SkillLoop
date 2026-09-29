namespace SkillLoop.Domain.Entities
{
    public class Slot
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public int Capacity { get; set; }

        public Course Course { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
