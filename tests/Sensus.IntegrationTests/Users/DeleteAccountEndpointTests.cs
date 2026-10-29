using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.DeleteAccount;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.IntegrationTests.Infrastructure;

namespace Sensus.IntegrationTests.Users;

public class DeleteAccountEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public DeleteAccountEndpointTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _factory.UserRepository.ClearReceivedCalls();
        _factory.ComercioRepository.ClearReceivedCalls();
        _factory.PasswordResetTokenRepository.ClearReceivedCalls();
    }

    private string CreateTokenForUser(User user)
    {
        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (token, _) = tokenGenerator.GenerateToken(user);
        return token;
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnNoContent_WhenPasswordIsCorrect()
    {
        // Arrange
        var passwordHasher = _factory.Services.GetRequiredService<IPasswordHasher>();
        var rawPassword = "ValidPassword123#";
        var realHash = passwordHasher.HashPassword(rawPassword);

        var user = User.Create(
            "User to Delete",
            "delete.me@sensus.com",
            realHash,
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var token = CreateTokenForUser(user);

        var command = new DeleteAccountCommand(rawPassword);

        using var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/users/account")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        await _factory.ComercioRepository.Received(1).DeleteByUsuarioIdAsync(user.Id, Arg.Any<CancellationToken>());
        await _factory.PasswordResetTokenRepository.Received(1).InvalidateExistingTokensForUserAsync(user.Id, Arg.Any<CancellationToken>());
        await _factory.UserRepository.Received(1).DeleteAsync(user.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnUnauthorized_WhenPasswordIsIncorrect()
    {
        // Arrange
        var passwordHasher = _factory.Services.GetRequiredService<IPasswordHasher>();
        var realHash = passwordHasher.HashPassword("RealPassword123#");

        var user = User.Create(
            "User Wrong Pass",
            "wrong.pass@sensus.com",
            realHash,
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var token = CreateTokenForUser(user);

        var command = new DeleteAccountCommand("IncorrectPassword123#");

        using var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/users/account")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert: 401 Unauthorized y no se borra ningún dato
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        await _factory.UserRepository.DidNotReceiveWithAnyArgs().DeleteAsync(default, default);
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnBadRequest_WhenPasswordIsEmpty()
    {
        var user = User.Create(
            "User Empty Pass",
            "empty.pass@sensus.com",
            "hashed-secret",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var token = CreateTokenForUser(user);

        var command = new DeleteAccountCommand("");

        using var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/users/account")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnForbidden_WhenCallerIsGuest()
    {
        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (guestToken, _) = tokenGenerator.GenerateGuestToken();

        var command = new DeleteAccountCommand("AnyPassword123#");

        using var request = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/users/account")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", guestToken);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
