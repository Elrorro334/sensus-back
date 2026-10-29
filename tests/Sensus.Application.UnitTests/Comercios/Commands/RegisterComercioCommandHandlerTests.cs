using FluentAssertions;
using NSubstitute;
using Sensus.Application.Comercios.Commands.RegisterComercio;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.Domain.Repositories;

namespace Sensus.Application.UnitTests.Comercios.Commands;

public class RegisterComercioCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IComercioRepository _comercioRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGeocodingService _geocodingService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly RegisterComercioCommandHandler _handler;

    private readonly Guid _testUserId = Guid.NewGuid();

    public RegisterComercioCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _comercioRepository = Substitute.For<IComercioRepository>();
        _currentUserService = Substitute.For<ICurrentUserService>();
        _geocodingService = Substitute.For<IGeocodingService>();
        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();

        _dateTimeProvider.UtcNow.Returns(new DateTime(2026, 10, 22, 12, 0, 0, DateTimeKind.Utc));

        _currentUserService.IsAuthenticated.Returns(true);
        _currentUserService.UserId.Returns(_testUserId.ToString());

        _handler = new RegisterComercioCommandHandler(
            _userRepository,
            _comercioRepository,
            _currentUserService,
            _geocodingService,
            _jwtTokenGenerator,
            _dateTimeProvider);
    }

    [Fact]
    public async Task Handle_Should_RegisterComercio_AndUpdateUserRoleToComerciante_WhenValid()
    {
        // Arrange
        var user = User.Create(
            "Oscar Salinas",
            "oscar@sensus.com",
            "hashed_pw",
            true,
            DateTime.UtcNow).Value;

        // Set user id to match test user id
        typeof(User).GetProperty("Id")?.SetValue(user, _testUserId);

        _userRepository.GetByIdAsync(_testUserId, Arg.Any<CancellationToken>())
            .Returns(user);

        _comercioRepository.ExistsByUsuarioIdAsync(_testUserId, Arg.Any<CancellationToken>())
            .Returns(false);

        _geocodingService.GeocodeAddressAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((19.432608, -99.133209));

        _jwtTokenGenerator.GenerateToken(Arg.Any<User>())
            .Returns(("token_with_comerciante", DateTime.UtcNow.AddHours(24)));

        var command = new RegisterComercioCommand(
            "Mi Tienda Express",
            "Av. Principal 123",
            "03940");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.EsComerciante.Should().BeTrue();
        result.Value.Roles.Should().Contain(RoleType.Comerciante);
        result.Value.Comercio.NombreComercial.Should().Be("Mi Tienda Express");
        result.Value.Comercio.Latitud.Should().Be(19.432608);
        result.Value.Comercio.Longitud.Should().Be(-99.133209);
        result.Value.Token.Should().Be("token_with_comerciante");

        // Verify dynamic update in repository
        await _comercioRepository.Received(1).AddAsync(Arg.Any<Comercio>(), Arg.Any<CancellationToken>());
        await _userRepository.Received(1).UpdateAsync(Arg.Is<User>(u => u.HasRole(RoleType.Comerciante)), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnConflict_WhenUserAlreadyHasComercio()
    {
        // Arrange
        var user = User.Create("Oscar", "oscar@sensus.com", "hash", true, DateTime.UtcNow).Value;
        _userRepository.GetByIdAsync(_testUserId, Arg.Any<CancellationToken>()).Returns(user);

        _comercioRepository.ExistsByUsuarioIdAsync(_testUserId, Arg.Any<CancellationToken>())
            .Returns(true);

        var command = new RegisterComercioCommand("Segunda Tienda", "Calle 2", "03940");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Comercio.AlreadyExists");
        await _comercioRepository.DidNotReceive().AddAsync(Arg.Any<Comercio>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_ReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        _currentUserService.IsAuthenticated.Returns(false);
        var command = new RegisterComercioCommand("Tienda", "Direccion", "03940");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.Unauthorized");
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _userRepository.GetByIdAsync(_testUserId, Arg.Any<CancellationToken>()).Returns((User?)null);
        var command = new RegisterComercioCommand("Tienda", "Direccion", "03940");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("User.NotFound");
    }
}
