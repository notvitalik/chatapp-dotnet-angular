using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChatApp.Application.DTOs.Auth;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Tests.Auth;

public sealed class AuthEndpointsTests : IClassFixture<AuthApiFactory>
{
    private readonly AuthApiFactory _factory;

    public AuthEndpointsTests(AuthApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Register_Success_ReturnsTokenAndUser()
    {
        using var client = _factory.CreateClient();
        var request = new RegisterRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com",
            Password = "Password1"
        };

        var response = await client.PostAsJsonAsync("/api/auth/register", request);

        await EnsureSuccessAsync(response);
        var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(payload);
        Assert.False(string.IsNullOrWhiteSpace(payload.Token));
        Assert.Equal("ada@example.com", payload.User.Email);
        Assert.Equal("Ada", payload.User.FirstName);
    }

    [Fact]
    public async Task Register_DuplicateRegistration_IsRejected()
    {
        using var client = _factory.CreateClient();
        var request = new RegisterRequest
        {
            FirstName = "Grace",
            LastName = "Hopper",
            Email = "grace@example.com",
            Password = "Password1"
        };

        var firstResponse = await client.PostAsJsonAsync("/api/auth/register", request);
        await EnsureSuccessAsync(firstResponse);

        var duplicateResponse = await client.PostAsJsonAsync("/api/auth/register", request);

        Assert.Equal(HttpStatusCode.BadRequest, duplicateResponse.StatusCode);
    }

    [Fact]
    public async Task Login_Success_ReturnsToken()
    {
        using var client = _factory.CreateClient();
        var registerRequest = new RegisterRequest
        {
            FirstName = "Linus",
            LastName = "Torvalds",
            Email = "linus@example.com",
            Password = "Password1"
        };
        await client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginRequest
            {
                Email = "linus@example.com",
                Password = "Password1"
            });

        await EnsureSuccessAsync(loginResponse);
        var payload = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(payload);
        Assert.False(string.IsNullOrWhiteSpace(payload.Token));
        Assert.Equal("linus@example.com", payload.User.Email);
    }

    [Fact]
    public async Task Login_InvalidPassword_IsRejected()
    {
        using var client = _factory.CreateClient();
        var registerRequest = new RegisterRequest
        {
            FirstName = "Margaret",
            LastName = "Hamilton",
            Email = "margaret@example.com",
            Password = "Password1"
        };
        await client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginRequest
            {
                Email = "margaret@example.com",
                Password = "WrongPassword1"
            });

        Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);
    }

    [Fact]
    public async Task Me_WithBearerToken_ReturnsCurrentUser()
    {
        using var client = _factory.CreateClient();
        var registerResponse = await client.PostAsJsonAsync(
            "/api/auth/register",
            new RegisterRequest
            {
                FirstName = "Katherine",
                LastName = "Johnson",
                Email = "katherine@example.com",
                Password = "Password1"
            });

        var auth = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        var meResponse = await client.GetAsync("/api/auth/me");

        await EnsureSuccessAsync(meResponse);
        var payload = await meResponse.Content.ReadFromJsonAsync<UserDto>();

        Assert.NotNull(payload);
        Assert.Equal("katherine@example.com", payload.Email);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync();
        throw new Xunit.Sdk.XunitException($"Unexpected status {(int)response.StatusCode}: {content}");
    }
}
