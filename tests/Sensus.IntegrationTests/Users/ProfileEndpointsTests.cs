using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.Commands.UpdateProfile;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.IntegrationTests.Infrastructure;

namespace Sensus.IntegrationTests.Users;

public class ProfileEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProfileEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private string CreateTokenForUser(User user)
    {
        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (token, _) = tokenGenerator.GenerateToken(user);
        return token;
    }

    [Fact]
    public async Task GetProfile_ShouldReturnUnauthorized_WhenNoTokenProvided()
    {
        var response = await _client.GetAsync("/api/v1/users/profile");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetProfile_ShouldReturnProfile_WhenAuthorized()
    {
        // Arrange
        var user = User.Create(
            "Juan Perez",
            "juan.perez@sensus.com",
            "hashed-pw",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var token = CreateTokenForUser(user);

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/users/profile");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<UserDto>();
        dto.Should().NotBeNull();
        dto!.NombreCompleto.Should().Be("Juan Perez");
        dto.Correo.Should().Be("juan.perez@sensus.com");
    }

    [Fact]
    public async Task UpdateProfile_ShouldUpdateNameAndFoto_WhenValid()
    {
        // Arrange
        var user = User.Create(
            "Original Name",
            "original@sensus.com",
            "hashed-pw",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var token = CreateTokenForUser(user);

        var command = new UpdateProfileCommand(
            "Updated Name",
            "https://res.cloudinary.com/sensus/image/upload/v12345/avatar.jpg");

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/users/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedDto = await response.Content.ReadFromJsonAsync<UserDto>();
        updatedDto.Should().NotBeNull();
        updatedDto!.NombreCompleto.Should().Be("Updated Name");
        updatedDto.FotoPerfilUrl.Should().Be("https://res.cloudinary.com/sensus/image/upload/v12345/avatar.jpg");

        await _factory.UserRepository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateProfile_ShouldReturnBadRequest_WhenUrlIsNotSecureCloudinary()
    {
        // Arrange
        var user = User.Create(
            "User Insecure",
            "insecure@sensus.com",
            "hashed-pw",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var token = CreateTokenForUser(user);

        var command = new UpdateProfileCommand(
            "Updated Name",
            "http://insecure-domain.com/photo.png");

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/users/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateProfile_ShouldReturnForbidden_WhenCallerIsGuest()
    {
        // Arrange: Crear token de Invitado
        var guestUser = User.Create(
            "Guest User",
            "guest@sensus.com",
            "pw",
            true,
            DateTime.UtcNow).Value;

        // Remover rol consumidor y asignar invitado
        typeof(User).GetField("_roles", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(guestUser, new List<string> { RoleType.Invitado });

        var token = CreateTokenForUser(guestUser);

        var command = new UpdateProfileCommand("Trying to Edit", null);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/users/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert: Política RegisteredUser rechaza al invitado con 403 Forbidden
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PhotoSignature_ShouldReturnSignature_WhenAuthorized()
    {
        // Arrange
        var user = User.Create(
            "User Signature",
            "sig@sensus.com",
            "pw",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>())
            .Returns(user);

        var token = CreateTokenForUser(user);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/users/photo-signature");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var sigResponse = await response.Content.ReadFromJsonAsync<CloudinarySignatureResponse>();
        sigResponse.Should().NotBeNull();
        sigResponse!.Signature.Should().NotBeNullOrWhiteSpace();
        sigResponse.Folder.Should().Contain(user.Id.ToString());
    }
}
