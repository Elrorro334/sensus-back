using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Users.Commands.DeleteAccount;
using Sensus.Application.Users.Commands.UpdateProfile;
using Sensus.Application.Users.DTOs;
using Sensus.Domain.Entities;
using Sensus.E2ETests.Infrastructure;

namespace Sensus.E2ETests.Journeys;

public class GuestModeE2EJourneyTests : IClassFixture<SensusE2EWebApplicationFactory>
{
    private readonly SensusE2EWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GuestModeE2EJourneyTests(SensusE2EWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Guest_FullLifecycle_Session_2KmRadiusClamping_AndAccessRestrictions_SucceedsEndToEnd()
    {
        // Setup: Registrar comercios de prueba (uno a ~260m y otro a ~3000m)
        var usuario1 = User.Create("Dueño 1", "comercio1@sensus.com", "hash", true, DateTime.UtcNow).Value;
        var usuario2 = User.Create("Dueño 2", "comercio2@sensus.com", "hash", true, DateTime.UtcNow).Value;
        await _factory.UserRepository.AddAsync(usuario1);
        await _factory.UserRepository.AddAsync(usuario2);

        // Centro: 19.432608, -99.133208
        // Cerca (~260m): 19.435000, -99.133208
        // Lejos (~3000m): 19.460000, -99.133208
        var comercioCerca = Comercio.Create(usuario1.Id, "Café Cercano", "Calle Cercana 10", "06000", 19.435000, -99.133208).Value;
        var comercioLejos = Comercio.Create(usuario2.Id, "Tienda Lejana", "Avenida Lejana 500", "07000", 19.460000, -99.133208).Value;

        await _factory.ComercioRepository.AddAsync(comercioCerca);
        await _factory.ComercioRepository.AddAsync(comercioLejos);

        // 1. SE-008: Iniciar sesión como invitado
        var guestRes = await _client.PostAsync("/api/v1/auth/guest", null);
        guestRes.StatusCode.Should().Be(HttpStatusCode.OK);

        var guestData = await guestRes.Content.ReadFromJsonAsync<GuestSessionResponseDto>();
        guestData.Should().NotBeNull();
        guestData!.Token.Should().NotBeNullOrWhiteSpace();
        guestData.MaxRadiusMeters.Should().Be(2000);
        guestData.Role.Should().Be("Invitado");

        var guestToken = guestData.Token;

        // 2. SE-008: Consultar comercios cercanos solicitando radio de 5,000 metros
        // El servidor DEBE aplicar la política de límite estricto de 2,000 metros para Invitado
        using var nearbyReq = new HttpRequestMessage(HttpMethod.Get, "/api/v1/comercios/nearby?lat=19.432608&lng=-99.133208&radioMetros=5000");
        nearbyReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", guestToken);
        var nearbyRes = await _client.SendAsync(nearbyReq);
        nearbyRes.StatusCode.Should().Be(HttpStatusCode.OK);

        var comercios = await nearbyRes.Content.ReadFromJsonAsync<List<ComercioDto>>();
        comercios.Should().NotBeNull();
        // Solo debe retornar el cercano (<2km), nunca el lejano (>2km)
        comercios!.Should().ContainSingle(c => c.NombreComercial == "Café Cercano");
        comercios.Should().NotContain(c => c.NombreComercial == "Tienda Lejana");

        // 3. SE-008: Intentar modificar perfil de usuario -> 403 Forbidden (solo RegisteredUser)
        using var editProfileReq = new HttpRequestMessage(HttpMethod.Put, "/api/v1/users/profile")
        {
            Content = JsonContent.Create(new UpdateProfileCommand("Guest Hack", null))
        };
        editProfileReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", guestToken);
        var editProfileRes = await _client.SendAsync(editProfileReq);
        editProfileRes.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        // 4. SE-008: Intentar eliminar cuenta -> 403 Forbidden
        using var deleteReq = new HttpRequestMessage(HttpMethod.Delete, "/api/v1/users/account")
        {
            Content = JsonContent.Create(new DeleteAccountCommand("anything"))
        };
        deleteReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", guestToken);
        var deleteRes = await _client.SendAsync(deleteReq);
        deleteRes.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        // 5. SE-008: Intentar actualizar comercio -> 403 Forbidden (solo MerchantOnly)
        using var editMerchantReq = new HttpRequestMessage(HttpMethod.Put, "/api/v1/merchants/profile")
        {
            Content = JsonContent.Create(new { Direccion = "Calle 123", CodigoPostal = "06000" })
        };
        editMerchantReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", guestToken);
        var editMerchantRes = await _client.SendAsync(editMerchantReq);
        editMerchantRes.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
