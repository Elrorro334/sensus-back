namespace Sensus.Application.Users.DTOs;

public sealed record AuthResponseDto(
    string Token,
    string TokenType,
    DateTime ExpiresAtUtc,
    UserDto User);
