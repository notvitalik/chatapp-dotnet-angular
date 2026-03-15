namespace ChatApp.Application.DTOs.Auth;

public sealed record AuthResponse(string Token, DateTimeOffset ExpiresAt, UserDto User);
