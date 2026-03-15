namespace ChatApp.Application.DTOs.Auth;

public sealed record UserDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? LastSeenAtUtc,
    bool IsActive);
