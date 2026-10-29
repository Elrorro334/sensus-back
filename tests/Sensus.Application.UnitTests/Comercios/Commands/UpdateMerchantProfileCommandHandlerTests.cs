using FluentAssertions;
using NSubstitute;
using Sensus.Application.Comercios.Commands.UpdateMerchantProfile;
using Sensus.Application.Comercios.DTOs;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Common;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Comercios.Commands;

public class UpdateMerchantProfileCommandHandlerTests
{
    private readonly IComercioRepository _comercioRepository = Substitute.For<IComercioRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly IGeocodingService _geocodingService = Substitute.For<IGeocodingService>();
    private readonly UpdateMerchantProfileCommandHandler _handler;

    public UpdateMerchantProfileCommandHandlerTests()
    {
        _handler = new UpdateMerchantProfileCommandHandler(
            _comercioRepository,
            _currentUserService,
            _geocodingService);
    }

    [Fact]
    public async Task Handle_ShouldRecalculateLocation_WhenPhysicalAddressChanges()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());

        var existingComercio = Comercio.Create(
            userId,
            "Viejo Nombre",
            "Calle Antigua 100",
            "01000",
            19.0,
            -99.0).Value;

        _comercioRepository.GetByUsuarioIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(existingComercio);

        _geocodingService.GeocodeAddressAsync("Calle Nueva 200", "03100", Arg.Any<CancellationToken>())
            .Returns(Result.Success((19.4326, -99.1332)));

        var command = new UpdateMerchantProfileCommand(
            "Nuevo Nombre",
            "Calle Nueva 200",
            "03100",
            null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.NombreComercial.Should().Be("Nuevo Nombre");
        result.Value.Direccion.Should().Be("Calle Nueva 200");
        result.Value.Latitud.Should().Be(19.4326);
        result.Value.Longitud.Should().Be(-99.1332);

        await _geocodingService.Received(1).GeocodeAddressAsync("Calle Nueva 200", "03100", Arg.Any<CancellationToken>());
        await _comercioRepository.Received(1).UpdateAsync(existingComercio, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldNotCallGeocoding_WhenAddressDidNotChange()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());

        var existingComercio = Comercio.Create(
            userId,
            "Tienda Existente",
            "Calle Misma 100",
            "01000",
            19.5,
            -99.5).Value;

        _comercioRepository.GetByUsuarioIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(existingComercio);

        var command = new UpdateMerchantProfileCommand(
            "Nombre Actualizado Solamente",
            "Calle Misma 100",
            "01000",
            null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Latitud.Should().Be(19.5);
        result.Value.Longitud.Should().Be(-99.5);

        await _geocodingService.DidNotReceiveWithAnyArgs().GeocodeAddressAsync(default!, default!, default);
        await _comercioRepository.Received(1).UpdateAsync(existingComercio, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnUnprocessable_WhenGeocodingFailsOnAddressChange()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUserService.UserId.Returns(userId.ToString());

        var existingComercio = Comercio.Create(
            userId,
            "Tienda",
            "Direccion Vieja 1",
            "01000",
            19.0,
            -99.0).Value;

        _comercioRepository.GetByUsuarioIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(existingComercio);

        _geocodingService.GeocodeAddressAsync("Direccion Inexistente 999", "99999", Arg.Any<CancellationToken>())
            .Returns(Result.Failure<(double, double)>(Error.Unprocessable("Geocoding.NotFound", "Dirección no encontrada")));

        var command = new UpdateMerchantProfileCommand(
            "Tienda",
            "Direccion Inexistente 999",
            "99999",
            null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Unprocessable);
        await _comercioRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default!, default);
    }
}
