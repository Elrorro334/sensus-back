using FluentAssertions;
using NSubstitute;
using Sensus.Application.Sensors.Commands.CreateSensor;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Sensors.Commands;

public class CreateSensorCommandHandlerTests
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateSensorCommandHandler _handler;

    public CreateSensorCommandHandlerTests()
    {
        _sensorRepository = Substitute.For<ISensorRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateSensorCommandHandler(_sensorRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateSensorCommand(
            "TEMP-TEST-01",
            "Sensor de Prueba",
            "Descripción de prueba",
            SensorType.Temperature);

        _sensorRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>())
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Code.Should().Be("TEMP-TEST-01");
        result.Value.Name.Should().Be("Sensor de Prueba");

        await _sensorRepository.Received(1).AddAsync(Arg.Any<Sensor>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnConflict_WhenCodeAlreadyExists()
    {
        // Arrange
        var command = new CreateSensorCommand(
            "TEMP-EXISTING",
            "Sensor Existente",
            "Descripción",
            SensorType.Temperature);

        _sensorRepository.ExistsByCodeAsync(command.Code, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Sensor.CodeExists");

        await _sensorRepository.DidNotReceive().AddAsync(Arg.Any<Sensor>(), Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
