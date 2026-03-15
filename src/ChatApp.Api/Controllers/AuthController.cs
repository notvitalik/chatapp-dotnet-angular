using System.Security.Claims;
using ChatApp.Application.DTOs.Auth;
using ChatApp.Application.Interfaces.Auth;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ITokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var normalizedEmail = request.Email.Trim();
        var existingUser = await userManager.FindByEmailAsync(normalizedEmail);
        if (existingUser is not null)
        {
            ModelState.AddModelError(nameof(request.Email), "A user with this email already exists.");
            return ValidationProblem(ModelState);
        }

        var user = new ApplicationUser
        {
            UserName = normalizedEmail,
            Email = normalizedEmail,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            CreatedAtUtc = DateTimeOffset.UtcNow,
            LastSeenAtUtc = DateTimeOffset.UtcNow,
            IsActive = true,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(ModelState);
        }

        var response = tokenService.CreateToken(user);
        return Ok(response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null || !user.IsActive)
        {
            return Unauthorized(new ValidationProblemDetails
            {
                Title = "Authentication failed.",
                Detail = "Invalid email or password."
            });
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Unauthorized(new ValidationProblemDetails
            {
                Title = "Authentication failed.",
                Detail = "Invalid email or password."
            });
        }

        user.LastSeenAtUtc = DateTimeOffset.UtcNow;
        await userManager.UpdateAsync(user);

        return Ok(tokenService.CreateToken(user));
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new UserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email ?? string.Empty,
            user.CreatedAtUtc,
            user.LastSeenAtUtc,
            user.IsActive));
    }
}
