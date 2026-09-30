namespace SkillLoop.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string? Headline { get; set; }
        public string? City { get; set; }
        public bool PhoneVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<ExternalLogin> ExternalLogins { get; set; } = new List<ExternalLogin>();
        public ICollection<OtpCode> OtpCodes { get; set; } = new List<OtpCode>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<SavedCourse> SavedCourses { get; set; } = new List<SavedCourse>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Conversation> TeachingConversations { get; set; } = new List<Conversation>();
        public ICollection<Conversation> LearningConversations { get; set; } = new List<Conversation>();
        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<PaymentOrder> PaymentOrders { get; set; } = new List<PaymentOrder>();
        public ICollection<PromoRedemption> PromoRedemptions { get; set; } = new List<PromoRedemption>();
        public Wallet Wallet { get; set; } = null!;
    }
}
