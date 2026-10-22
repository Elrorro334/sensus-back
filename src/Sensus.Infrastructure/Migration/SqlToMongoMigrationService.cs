using Microsoft.Extensions.Logging;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Infrastructure.Migration;

/// <summary>
/// Servicio de migración que traslada los datos definidos en scripts SQL hacia MongoDB Atlas,
/// aplicando hashing seguro BCrypt y el modelo de documentos unificado.
/// </summary>
public sealed class SqlToMongoMigrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<SqlToMongoMigrationService> _logger;

    public SqlToMongoMigrationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ILogger<SqlToMongoMigrationService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta la migración inicial del usuario semilla provisto en Base_Datos_Movi;.sql.
    /// </summary>
    public async Task MigrateInitialDataAsync()
    {
        const string initialEmail = "oscarsa48700@gmail.com";

        var exists = await _userRepository.ExistsByEmailAsync(initialEmail);
        if (exists)
        {
            _logger.LogInformation("Los datos iniciales de SQL ya se encuentran migrados en MongoDB.");
            return;
        }

        _logger.LogInformation("Iniciando migración de datos desde SQL (Base_Datos_Movi;.sql) hacia MongoDB Atlas...");

        // En el SQL original: Values('oscar jose salinas','oscarsa48700@gmail.com','123456789',1,'15-10-2026',1,'15-10-2026','15-10-2026')
        // Hasheamos la contraseña con BCrypt cumpliendo el estándar de seguridad de SE-002:
        var securePasswordHash = _passwordHasher.HashPassword("Password123!");

        var migratedUser = User.Create(
            "Oscar Jose Salinas",
            initialEmail,
            securePasswordHash,
            true,
            DateTime.UtcNow).Value;

        await _userRepository.AddAsync(migratedUser);

        _logger.LogInformation(
            "Usuario migrado exitosamente hacia MongoDB Atlas: {Email} con contraseña segura por defecto ('Password123!') y rol Consumidor.",
            initialEmail);
    }
}
