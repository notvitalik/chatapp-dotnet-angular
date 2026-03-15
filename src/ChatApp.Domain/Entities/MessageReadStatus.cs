using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

public sealed class MessageReadStatus : BaseAuditableEntity
{
    public Guid MessageId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTimeOffset ReadAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public Message? Message { get; set; }
    public ApplicationUser? User { get; set; }
}
