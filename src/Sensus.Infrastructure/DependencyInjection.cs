using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // Configuración de MongoDB Atlas
        services.Configure<MongoDbSettings>(
            configuration.GetSection(MongoDbSettings.SectionName));
        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IUserRepository, MongoUserRepository>();

        // Servicio de migración de SQL a MongoDB Atlas
        services.AddScoped<SqlToMongoMigrationService>();

        return services;
    }
}
