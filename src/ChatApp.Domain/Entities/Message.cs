using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

public sealed class Message : BaseAuditableEntity
{
    public Guid ConversationId { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset SentAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public Conversation? Conversation { get; set; }
    public ApplicationUser? Sender { get; set; }
    public ICollection<MessageReadStatus> ReadStatuses { get; set; } = new List<MessageReadStatus>();
}
