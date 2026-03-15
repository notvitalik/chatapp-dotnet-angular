namespace ChatApp.Application.DTOs.Conversations;

public sealed record ConversationSummaryDto(Guid Id, string Name, DateTimeOffset UpdatedAtUtc);
