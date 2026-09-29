using SkillLoop.Domain.Enums;

namespace SkillLoop.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid EnrollmentId { get; set; }
        public Guid SlotId { get; set; }
        public SessionType SessionType { get; set; }
        public int Price { get; set; }
        public BookingStatus Status { get; set; }
        public string? MeetingLink { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        public Enrollment Enrollment { get; set; } = null!;
        public Slot Slot { get; set; } = null!;

        public ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}
