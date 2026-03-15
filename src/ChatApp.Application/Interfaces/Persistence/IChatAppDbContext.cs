using ChatApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Application.Interfaces.Persistence;

public interface IChatAppDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Conversation> Conversations { get; }
    DbSet<ConversationParticipant> ConversationParticipants { get; }
    DbSet<Message> Messages { get; }
    DbSet<MessageReadStatus> MessageReadStatuses { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
