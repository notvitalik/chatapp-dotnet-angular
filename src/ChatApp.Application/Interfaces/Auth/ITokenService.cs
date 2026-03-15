using ChatApp.Application.DTOs.Auth;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.Interfaces.Auth;

public interface ITokenService
{
    AuthResponse CreateToken(ApplicationUser user);
}
