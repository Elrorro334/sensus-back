using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sensus.Application.Comercios.Commands.UpdateMerchantProfile;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.IntegrationTests.Infrastructure;

namespace Sensus.IntegrationTests.Merchants;

public class MerchantProfileEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public MerchantProfileEndpointsTests(CustomWebApplicationFactory factory)
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
    public async Task UpdateMerchantProfile_ShouldSucceed_WhenUserIsMerchant()
    {
        // Arrange
        var merchantUser = User.Create(
            "Dueño Comercio",
            "dueno@comercio.com",
            "hashed-pw",
            true,
            DateTime.UtcNow).Value;

        merchantUser.AddRole(RoleType.Comerciante);

        _factory.UserRepository.GetByIdAsync(merchantUser.Id, Arg.Any<CancellationToken>())
            .Returns(merchantUser);

        var token = CreateTokenForUser(merchantUser);

        var existingComercio = Comercio.Create(
            merchantUser.Id,
            "Mi Tiendita",
            "Calle Antigua 123",
            "01000",
            19.4000,
            -99.1500).Value;

        _factory.ComercioRepository.GetByUsuarioIdAsync(merchantUser.Id, Arg.Any<CancellationToken>())
            .Returns(existingComercio);

        _factory.FakeGeocoding.SetupCoordinates(19.4326, -99.1332);

        var command = new UpdateMerchantProfileCommand(
            "Mi Tiendita Renovada",
            "Av. Reforma 222",
            "06600",
            [
                new HorarioDiaDto(DayOfWeek.Monday, "08:00", "20:00", false),
                new HorarioDiaDto(DayOfWeek.Sunday, null, null, true)
            ]);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/merchants/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<ComercioDto>();
        dto.Should().NotBeNull();
        dto!.NombreComercial.Should().Be("Mi Tiendita Renovada");
        dto.Direccion.Should().Be("Av. Reforma 222");
        dto.CodigoPostal.Should().Be("06600");
        dto.Latitud.Should().Be(19.4326);
        dto.Longitud.Should().Be(-99.1332);
        dto.Horarios.Should().HaveCount(2);

        await _factory.ComercioRepository.Received(1).UpdateAsync(existingComercio, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateMerchantProfile_ShouldReturnForbidden_WhenUserIsConsumerOnly()
    {
        // Consumidor sin rol comerciante
        var consumerUser = User.Create(
            "Consumidor Normal",
            "consumidor@sensus.com",
            "pw",
            true,
            DateTime.UtcNow).Value;

        _factory.UserRepository.GetByIdAsync(consumerUser.Id, Arg.Any<CancellationToken>())
            .Returns(consumerUser);

        var token = CreateTokenForUser(consumerUser);

        var command = new UpdateMerchantProfileCommand(
            "No Autorizado",
            "Av. Sin Permiso 123",
            "03100",
            null);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/merchants/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateMerchantProfile_ShouldReturnForbidden_WhenUserIsGuest()
    {
        var guestUser = User.Create(
            "Invitado",
            "guest@sensus.com",
            "pw",
            true,
            DateTime.UtcNow).Value;

        typeof(User).GetField("_roles", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(guestUser, new List<string> { RoleType.Invitado });

        var token = CreateTokenForUser(guestUser);

        var command = new UpdateMerchantProfileCommand(
            "Intento Invitado",
            "Calle 123",
            "01000",
            null);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/merchants/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateMerchantProfile_ShouldReturnNotFound_WhenMerchantHasNoComercio()
    {
        var merchantUser = User.Create(
            "Comerciante Sin Local",
            "sinlocal@comercio.com",
            "pw",
            true,
            DateTime.UtcNow).Value;

        merchantUser.AddRole(RoleType.Comerciante);

        _factory.UserRepository.GetByIdAsync(merchantUser.Id, Arg.Any<CancellationToken>())
            .Returns(merchantUser);

        _factory.ComercioRepository.GetByUsuarioIdAsync(merchantUser.Id, Arg.Any<CancellationToken>())
            .Returns((Comercio?)null);

        var token = CreateTokenForUser(merchantUser);

        var command = new UpdateMerchantProfileCommand(
            "Mi Tienda Inexistente",
            "Calle 123",
            "01000",
            null);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/merchants/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateMerchantProfile_ShouldReturnUnprocessable_WhenAddressCannotBeGeocoded()
    {
        var merchantUser = User.Create(
            "Comerciante",
            "failgeo@comercio.com",
            "pw",
            true,
            DateTime.UtcNow).Value;

        merchantUser.AddRole(RoleType.Comerciante);

        _factory.UserRepository.GetByIdAsync(merchantUser.Id, Arg.Any<CancellationToken>())
            .Returns(merchantUser);

        var existingComercio = Comercio.Create(
            merchantUser.Id,
            "Tienda",
            "Calle Antigua 1",
            "01000",
            19.0,
            -99.0).Value;

        _factory.ComercioRepository.GetByUsuarioIdAsync(merchantUser.Id, Arg.Any<CancellationToken>())
            .Returns(existingComercio);

        _factory.FakeGeocoding.SetupFailure();

        var token = CreateTokenForUser(merchantUser);

        var command = new UpdateMerchantProfileCommand(
            "Tienda",
            "Calle Inexistente En el Mapa 9999",
            "99999",
            null);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/merchants/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task UpdateMerchantProfile_ShouldReturnBadRequest_WhenZipCodeIsInvalid()
    {
        var merchantUser = User.Create(
            "Comerciante",
            "badcp@comercio.com",
            "pw",
            true,
            DateTime.UtcNow).Value;

        merchantUser.AddRole(RoleType.Comerciante);

        _factory.UserRepository.GetByIdAsync(merchantUser.Id, Arg.Any<CancellationToken>())
            .Returns(merchantUser);

        var token = CreateTokenForUser(merchantUser);

        var command = new UpdateMerchantProfileCommand(
            "Tienda",
            "Calle Correcta 123",
            "CP-INVALIDO",
            null);

        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/v1/merchants/profile")
        {
            Content = JsonContent.Create(command)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
