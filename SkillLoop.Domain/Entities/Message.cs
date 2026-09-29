namespace SkillLoop.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string Body { get; set; } = null!;
        public string? AttachmentUrl { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }

        public Conversation Conversation { get; set; } = null!;
        public User Sender { get; set; } = null!;
    }
}
