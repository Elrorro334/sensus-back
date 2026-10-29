using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Sensus.Domain.Enums;

namespace Sensus.Api.Extensions;

public static class SecurityExtensions
{
    public const string AuthStrictPolicy = "auth-strict";
    public const string RecoverPolicy = "recover";

    public const string RegisteredUserPolicy = "RegisteredUser";
    public const string MerchantOnlyPolicy = "MerchantOnly";
    public const string GuestOrRegisteredPolicy = "GuestOrRegistered";

    public static IServiceCollection AddSecurityRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy(AuthStrictPolicy, httpContext =>
            {
                var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: clientIp,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.AddPolicy(RecoverPolicy, httpContext =>
            {
                var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: clientIp,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(15),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });
        });

        return services;
    }

    public static IServiceCollection AddAppAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(RegisteredUserPolicy, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(ctx =>
                    !ctx.User.IsInRole(RoleType.Invitado));
            });

            options.AddPolicy(MerchantOnlyPolicy, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(RoleType.Comerciante);
                policy.RequireAssertion(ctx =>
                    !ctx.User.IsInRole(RoleType.Invitado));
            });

            options.AddPolicy(GuestOrRegisteredPolicy, policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

            await next();
        });
    }
}
