using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sensus.Api.Endpoints;
using Sensus.Api.Middlewares;
using Sensus.Api.Services;
using Sensus.Application;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Repositories;
using Sensus.Infrastructure;
using Sensus.Infrastructure.Authentication;
using Sensus.Infrastructure.Migration;
using Sensus.Infrastructure.Persistence;
using Sensus.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// 1. Capa de Aplicación
builder.Services.AddApplication();

// 2. Capa de Infraestructura (BCrypt, JWT, MongoDB Atlas, EF Core)
builder.Services.AddInfrastructure(builder.Configuration);

// 3. Capa de Presentación / API
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Configuración de Autenticación con JWT Bearer
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();
var secretKey = Encoding.UTF8.GetBytes(jwtSettings.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Manejo global de excepciones con ProblemDetails (RFC 7807)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Documentación OpenAPI
builder.Services.AddOpenApi();

// Health Checks
builder.Services.AddHealthChecks();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Pipeline HTTP
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Sembrado de entidades relacionales de prueba
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await ApplicationDbContextSeed.SeedSampleDataAsync(dbContext);

    // Sembrado del usuario Administrador (si se configuran variables de entorno)
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await AdminUserSeeder.SeedAdminUserAsync(userRepository, passwordHasher, app.Configuration, logger);
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Endpoint de verificación de salud
app.MapHealthChecks("/health");

// Registro de endpoints
app.MapAuthEndpoints();
app.MapSensorEndpoints();

app.Run();

// Requerido para WebApplicationFactory en pruebas
public partial class Program { }
