using ChatApp.Application.Interfaces.Persistence;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Persistence;

public sealed class AppDbContext : IdentityDbContext<ApplicationUser>, IChatAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<MessageReadStatus> MessageReadStatuses => Set<MessageReadStatus>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.CreatedAtUtc).IsRequired();
            entity.Property(x => x.IsActive).HasDefaultValue(true);
        });

        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        builder.Entity<Conversation>(entity =>
        {
            entity.ToTable("Conversations");
            entity.Property(x => x.Name).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Type).IsRequired();
        });

        builder.Entity<ConversationParticipant>(entity =>
        {
            entity.ToTable("ConversationParticipants");
            entity.HasIndex(x => new { x.ConversationId, x.UserId }).IsUnique();
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.HasOne(x => x.Conversation)
                .WithMany(x => x.Participants)
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.User)
                .WithMany(x => x.Conversations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Message>(entity =>
        {
            entity.ToTable("Messages");
            entity.Property(x => x.SenderId).HasMaxLength(450).IsRequired();
            entity.Property(x => x.Content).HasMaxLength(4000).IsRequired();
            entity.HasOne(x => x.Conversation)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Sender)
                .WithMany(x => x.SentMessages)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<MessageReadStatus>(entity =>
        {
            entity.ToTable("MessageReadStatuses");
            entity.HasIndex(x => new { x.MessageId, x.UserId }).IsUnique();
            entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
            entity.HasOne(x => x.Message)
                .WithMany(x => x.ReadStatuses)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.User)
                .WithMany(x => x.MessageReadStatuses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
