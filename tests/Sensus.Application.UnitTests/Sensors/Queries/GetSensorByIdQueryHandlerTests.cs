using FluentAssertions;
using NSubstitute;
using Sensus.Application.Sensors.Queries.GetSensorById;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Sensors.Queries;

public class GetSensorByIdQueryHandlerTests
{
    private readonly ISensorRepository _sensorRepository;
    private readonly GetSensorByIdQueryHandler _handler;

    public GetSensorByIdQueryHandlerTests()
    {
        _sensorRepository = Substitute.For<ISensorRepository>();
        _handler = new GetSensorByIdQueryHandler(_sensorRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenSensorDoesNotExist()
    {
        // Arrange
        var query = new GetSensorByIdQuery(Guid.NewGuid());

        _sensorRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns((Sensor?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Sensor.NotFound");
    }

    [Fact]
    public async Task Handle_Should_ReturnSensorDto_WhenSensorExists()
    {
        // Arrange
        var sensor = Sensor.Create("TEMP-100", "Sensor 100", "Desc", SensorType.Temperature).Value;
        var query = new GetSensorByIdQuery(sensor.Id);

        _sensorRepository.GetByIdAsync(query.Id, Arg.Any<CancellationToken>())
            .Returns(sensor);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(sensor.Id);
        result.Value.Code.Should().Be(sensor.Code);
    }
}
