using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.IntegrationTests.Infrastructure;

namespace Sensus.IntegrationTests.Security;

public class SecurityPipelineTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public SecurityPipelineTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthEndpoint_ShouldReturnSecurityHeaders()
    {
        var response = await _client.GetAsync("/health");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Contains("X-Content-Type-Options").Should().BeTrue();
        response.Headers.GetValues("X-Content-Type-Options").Should().Contain("nosniff");
        response.Headers.Contains("X-Frame-Options").Should().BeTrue();
        response.Headers.GetValues("X-Frame-Options").Should().Contain("DENY");
        response.Headers.Contains("Referrer-Policy").Should().BeTrue();
    }

    [Fact]
    public async Task MeEndpoint_ShouldSucceed_WhenTokenHasMatchingSecurityStamp()
    {
        var user = User.Create(
            "Test Security User",
            "security@sensus.com",
            "hashed-password",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (token, _) = tokenGenerator.GenerateToken(user);

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/auth/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task MeEndpoint_ShouldReturnUnauthorized_WhenSecurityStampHasBeenRotated()
    {
        var user = User.Create(
            "Revoked Session User",
            "revoked@sensus.com",
            "hashed-password",
            true,
            DateTime.UtcNow).Value;

        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (oldToken, _) = tokenGenerator.GenerateToken(user);

        // Rotar el security stamp (ej. tras cambio de contraseña o logout)
        user.RotateSecurityStamp();

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/auth/me");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oldToken);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LogoutEndpoint_ShouldReturnUnauthorized_WhenUnauthenticated()
    {
        // Act: POST /api/v1/auth/logout sin token
        var response = await _client.PostAsync("/api/v1/auth/logout", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LogoutEndpoint_ShouldReturnNoContent_AndInvalidateSubsequentRequests()
    {
        // Arrange
        var user = User.Create(
            "Active Logout User",
            "active.logout@sensus.com",
            "hashed-password",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (token, _) = tokenGenerator.GenerateToken(user);

        // Act 1: POST /api/v1/auth/logout con token válido
        using var logoutRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/logout");
        logoutRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var logoutResponse = await _client.SendAsync(logoutRequest);

        // Assert 1: Responde 204 NoContent
        logoutResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act 2: Intento de usar el mismo token en otro endpoint protegido (CA-004-02)
        using var meRequest = new HttpRequestMessage(HttpMethod.Get, "/api/v1/auth/me");
        meRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var meResponse = await _client.SendAsync(meRequest);

        // Assert 2: El token revocado es rechazado de inmediato
        meResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task LogoutEndpoint_ShouldBeIdempotent_WhenCalledMultipleTimes()
    {
        // Arrange
        var user = User.Create(
            "Idempotent Logout User",
            "idempotent.logout@sensus.com",
            "hashed-password",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (token, _) = tokenGenerator.GenerateToken(user);

        // Act 1: Primer logout
        using var request1 = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/logout");
        request1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response1 = await _client.SendAsync(request1);
        response1.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Act 2: Segundo logout consecutivo con el mismo token (CA-004-03)
        using var request2 = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/logout");
        request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response2 = await _client.SendAsync(request2);

        // Assert 2: Sigue respondiendo 204 sin error
        response2.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
