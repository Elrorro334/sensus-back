using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.Domain.Repositories;

namespace Sensus.Infrastructure.Migration;

public static class AdminUserSeeder
{
    public static async Task SeedAdminUserAsync(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        ILogger logger)
    {
        var adminEmail = configuration["AdminSeed:Email"] ?? Environment.GetEnvironmentVariable("ADMIN_SEED_EMAIL");
        var adminPassword = configuration["AdminSeed:Password"] ?? Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD");
        var adminName = configuration["AdminSeed:FullName"] ?? Environment.GetEnvironmentVariable("ADMIN_SEED_FULLNAME") ?? "Administrador Sensus";

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            logger.LogDebug("No se configuraron variables de entorno para AdminSeed. Omitiendo sembrado automático.");
            return;
        }

        try
        {
            var exists = await userRepository.ExistsByEmailAsync(adminEmail);
            if (exists)
            {
                logger.LogInformation("El usuario {Email} ya se encuentra registrado en el sistema.", adminEmail);
                return;
            }

            logger.LogInformation("Registrando usuario administrador inicial {Email}...", adminEmail);

            var passwordHash = passwordHasher.HashPassword(adminPassword);
            var userResult = User.Create(
                adminName,
                adminEmail,
                passwordHash,
                true,
                DateTime.UtcNow);

            if (userResult.IsSuccess)
            {
                var user = userResult.Value;
                user.AddRole(RoleType.Administrador);

                await userRepository.AddAsync(user);
                logger.LogInformation("¡Usuario administrador {Email} creado exitosamente!", adminEmail);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al verificar o sembrar el usuario administrador inicial.");
        }
    }
}
