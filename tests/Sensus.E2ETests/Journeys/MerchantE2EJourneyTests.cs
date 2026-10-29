using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Sensus.Application.Comercios.Commands.UpdateMerchantProfile;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Users.Commands.Login;
using Sensus.Application.Users.Commands.RegisterUser;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.E2ETests.Infrastructure;

namespace Sensus.E2ETests.Journeys;

public class MerchantE2EJourneyTests : IClassFixture<SensusE2EWebApplicationFactory>
{
    private readonly SensusE2EWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public MerchantE2EJourneyTests(SensusE2EWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Merchant_FullLifecycle_UpdateProfile_RecalculateGeo_AndSearch_SucceedsEndToEnd()
    {
        // 1. Registrar usuario
        var email = $"merchant.e2e.{Guid.NewGuid():N}@sensus.com";
        var password = "Password123#";
        var regRes = await _client.PostAsJsonAsync("/api/v1/auth/register", new RegisterUserCommand("Dueño Panadería", email, password, true));
        regRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var regData = await regRes.Content.ReadFromJsonAsync<AuthResponseDto>();
        var userId = regData!.User.Id;

        // 2. Asignar rol comerciante y crear comercio inicial
        var user = await _factory.UserRepository.GetByIdAsync(userId);
        user!.AddRole(RoleType.Comerciante);
        await _factory.UserRepository.UpdateAsync(user);

        var comercio = Comercio.Create(userId, "Panadería Antigua", "Calle Vieja 1", "06000", 19.430000, -99.130000).Value;
        await _factory.ComercioRepository.AddAsync(comercio);

        // 3. Login para obtener JWT con rol Comerciante
        var loginRes = await _client.PostAsJsonAsync("/api/v1/auth/login", new LoginCommand(email, password));
        loginRes.StatusCode.Should().Be(HttpStatusCode.OK);
        var authData = await loginRes.Content.ReadFromJsonAsync<AuthResponseDto>();
        var merchantToken = authData!.Token;

        // Configurar fake geocoding para la nueva dirección
        _factory.FakeGeocoding.SetupCoordinates(19.434000, -99.135000);

        // 4. SE-007: Actualizar datos del local (dirección y horarios)
        var updateCommand = new UpdateMerchantProfileCommand(
            "Panadería Artesanal Nueva",
            "Avenida Juárez 50",
            "06050",
            new List<HorarioDiaDto>
            {
                new(DayOfWeek.Monday, "08:00", "20:00", false),
                new(DayOfWeek.Tuesday, "08:00", "20:00", false),
                new(DayOfWeek.Sunday, "00:00", "00:00", true)
            });

        using var updateReq = new HttpRequestMessage(HttpMethod.Put, "/api/v1/merchants/profile")
        {
            Content = JsonContent.Create(updateCommand)
        };
        updateReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", merchantToken);
        var updateRes = await _client.SendAsync(updateReq);
        updateRes.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedComercio = await updateRes.Content.ReadFromJsonAsync<ComercioDto>();
        updatedComercio.Should().NotBeNull();
        updatedComercio!.NombreComercial.Should().Be("Panadería Artesanal Nueva");
        updatedComercio.Direccion.Should().Be("Avenida Juárez 50");
        updatedComercio.CodigoPostal.Should().Be("06050");
        updatedComercio.Latitud.Should().Be(19.434000);
        updatedComercio.Longitud.Should().Be(-99.135000);
        updatedComercio.Horarios.Should().HaveCount(3);

        // 5. SE-008: Búsqueda de proximidad encuentra el comercio con sus nuevas coordenadas
        using var nearbyReq = new HttpRequestMessage(HttpMethod.Get, "/api/v1/comercios/nearby?lat=19.434000&lng=-99.135000&radioMetros=1000");
        nearbyReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", merchantToken);
        var nearbyRes = await _client.SendAsync(nearbyReq);
        nearbyRes.StatusCode.Should().Be(HttpStatusCode.OK);

        var nearbyList = await nearbyRes.Content.ReadFromJsonAsync<List<ComercioDto>>();
        nearbyList.Should().NotBeNull();
        nearbyList!.Should().Contain(c => c.NombreComercial == "Panadería Artesanal Nueva");
    }
}
