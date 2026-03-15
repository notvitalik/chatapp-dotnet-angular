using ChatApp.Domain.Common;

namespace ChatApp.Domain.Entities;

public sealed class ConversationParticipant : BaseAuditableEntity
{
    public Guid ConversationId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public Conversation? Conversation { get; set; }
    public ApplicationUser? User { get; set; }
}
