using Microsoft.AspNetCore.Identity;

namespace ChatApp.Domain.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastSeenAtUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<ConversationParticipant> Conversations { get; set; } = new List<ConversationParticipant>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<MessageReadStatus> MessageReadStatuses { get; set; } = new List<MessageReadStatus>();
}
