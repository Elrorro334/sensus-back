using FluentAssertions;
using NSubstitute;
using Sensus.Application.Comercios.Queries.GetNearbyComercios;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Comercios.Queries;

public class GetNearbyComerciosQueryHandlerTests
{
    private readonly IComercioRepository _comercioRepository = Substitute.For<IComercioRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly GetNearbyComerciosQueryHandler _handler;

    public GetNearbyComerciosQueryHandlerTests()
    {
        _handler = new GetNearbyComerciosQueryHandler(_comercioRepository, _currentUserService);
    }

    [Fact]
    public async Task Handle_ShouldCapRadiusTo2000_WhenCallerIsGuest()
    {
        // Arrange
        _currentUserService.IsGuest.Returns(true);
        _comercioRepository.GetNearbyAsync(19.4326, -99.1332, 2000, Arg.Any<CancellationToken>())
            .Returns(new List<Comercio>());

        var query = new GetNearbyComerciosQuery(19.4326, -99.1332, RadioMetros: 5000);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _comercioRepository.Received(1).GetNearbyAsync(19.4326, -99.1332, 2000, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldAllowRequestedRadius_WhenCallerIsRegisteredUser()
    {
        // Arrange
        _currentUserService.IsGuest.Returns(false);
        _comercioRepository.GetNearbyAsync(19.4326, -99.1332, 5000, Arg.Any<CancellationToken>())
            .Returns(new List<Comercio>());

        var query = new GetNearbyComerciosQuery(19.4326, -99.1332, RadioMetros: 5000);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await _comercioRepository.Received(1).GetNearbyAsync(19.4326, -99.1332, 5000, Arg.Any<CancellationToken>());
    }
}
