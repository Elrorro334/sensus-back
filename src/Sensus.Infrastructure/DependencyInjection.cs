using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Repositories;
using Sensus.Infrastructure.Authentication;
using Sensus.Infrastructure.Migration;
using Sensus.Infrastructure.Persistence.Mongo;
using Sensus.Infrastructure.Persistence.Mongo.Repositories;
using Sensus.Infrastructure.Services;

namespace Sensus.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Servicios base
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // Autenticación, BCrypt y JWT
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidator>();
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .ValidateOnStart();

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // Geocodificación (SE-003)
        services.AddScoped<IGeocodingService, NominatimGeocodingService>();

        // Servicio de correo transaccional (SE-005 / Api.Mailer.Invitation)
        services.Configure<MailerSettings>(
            configuration.GetSection(MailerSettings.SectionName));
        services.AddHttpClient<IEmailSender, HttpMailerEmailSender>();

        // Almacenamiento y firma de fotos Cloudinary (SE-006)
        services.Configure<CloudinarySettings>(
            configuration.GetSection(CloudinarySettings.SectionName));
        services.AddSingleton<IProfileImageStorage, CloudinaryImageStorage>();

        // Configuración de MongoDB Atlas
        services.Configure<MongoDbSettings>(
            configuration.GetSection(MongoDbSettings.SectionName));
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IUserRepository, MongoUserRepository>();
        services.AddScoped<IComercioRepository, MongoComercioRepository>();
        services.AddScoped<IPasswordResetTokenRepository, MongoPasswordResetTokenRepository>();

        // Servicio de migración de SQL a MongoDB Atlas
        services.AddScoped<SqlToMongoMigrationService>();

        return services;
    }
}
