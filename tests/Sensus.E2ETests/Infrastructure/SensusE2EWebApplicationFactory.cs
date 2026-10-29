using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sensus.Application.Common.Interfaces;
using Sensus.Domain.Repositories;
using Sensus.TestKit.Fakes;

namespace Sensus.E2ETests.Infrastructure;

public class SensusE2EWebApplicationFactory : WebApplicationFactory<Program>
{
    public InMemoryUserRepository UserRepository { get; } = new();
    public InMemoryComercioRepository ComercioRepository { get; } = new();
    public InMemoryPasswordResetTokenRepository PasswordResetTokenRepository { get; } = new();
    public FakeProfileImageStorage FakeImageStorage { get; } = new();
    public FakeDateTimeProvider FakeClock { get; } = new();
    public FakeGeocodingService FakeGeocoding { get; } = new();
    public FakeEmailSender FakeEmailSender { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IUserRepository>();
            services.AddSingleton<IUserRepository>(UserRepository);

            services.RemoveAll<IComercioRepository>();
            services.AddSingleton<IComercioRepository>(ComercioRepository);

            services.RemoveAll<IPasswordResetTokenRepository>();
            services.AddSingleton<IPasswordResetTokenRepository>(PasswordResetTokenRepository);

            services.RemoveAll<IProfileImageStorage>();
            services.AddSingleton<IProfileImageStorage>(FakeImageStorage);

            services.RemoveAll<IDateTimeProvider>();
            services.AddSingleton<IDateTimeProvider>(FakeClock);

            services.RemoveAll<IGeocodingService>();
            services.AddSingleton<IGeocodingService>(FakeGeocoding);

            services.RemoveAll<IEmailSender>();
            services.AddSingleton<IEmailSender>(FakeEmailSender);
        });
    }
}
