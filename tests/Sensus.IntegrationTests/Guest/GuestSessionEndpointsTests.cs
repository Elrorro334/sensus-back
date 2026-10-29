using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sensus.Application.Comercios.Commands.RegisterComercio;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Common.Interfaces;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.IntegrationTests.Infrastructure;

namespace Sensus.IntegrationTests.Guest;

public class GuestSessionEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GuestSessionEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateGuestSession_ShouldReturnTokenWith2KmMaxRadius()
    {
        var response = await _client.PostAsync("/api/v1/auth/guest", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<GuestSessionResponseDto>();
        dto.Should().NotBeNull();
        dto!.Token.Should().NotBeNullOrWhiteSpace();
        dto.Role.Should().Be(RoleType.Invitado);
        dto.MaxRadiusMeters.Should().Be(2000);
    }

    [Fact]
    public async Task Nearby_ShouldCapTo2000Meters_WhenCallerIsGuest()
    {
        // Arrange: Crear sesión de invitado
        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (guestToken, _) = tokenGenerator.GenerateGuestToken();

        var dummyComercio = Comercio.Create(
            Guid.NewGuid(),
            "Cafetería Local",
            "Calle Cerca 100",
            "03100",
            19.4320,
            -99.1330).Value;

        _factory.ComercioRepository.GetNearbyAsync(19.4326, -99.1332, 2000, Arg.Any<CancellationToken>())
            .Returns(new List<Comercio> { dummyComercio });

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/v1/comercios/nearby?lat=19.4326&lng=-99.1332&radioMetros=5000"); // Pide 5000m
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", guestToken);

        // Act
        var response = await _client.SendAsync(request);

        // Assert: El endpoint responde 200 OK y el repositorio fue invocado con máximo 2000 metros
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var results = await response.Content.ReadFromJsonAsync<List<ComercioDto>>();
        results.Should().NotBeNull();
        results!.Should().HaveCount(1);
        results[0].NombreComercial.Should().Be("Cafetería Local");

        await _factory.ComercioRepository.Received(1)
            .GetNearbyAsync(19.4326, -99.1332, 2000, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RegisterComercio_ShouldReturnForbidden_WhenCallerIsGuest()
    {
        var tokenGenerator = _factory.Services.GetRequiredService<IJwtTokenGenerator>();
        var (guestToken, _) = tokenGenerator.GenerateGuestToken();

        var command = new RegisterComercioCommand("Intento Comercio", "Calle 1", "01000");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/comercios")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", guestToken);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
