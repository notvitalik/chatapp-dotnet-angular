using ChatApp.Domain.Common;
using ChatApp.Domain.Enums;

namespace ChatApp.Domain.Entities;

public sealed class Conversation : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public ConversationType Type { get; set; }
    public ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
