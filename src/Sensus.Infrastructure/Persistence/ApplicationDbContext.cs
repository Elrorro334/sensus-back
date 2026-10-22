using Microsoft.EntityFrameworkCore;
using Sensus.Domain.Entities;
using Sensus.Domain.Repositories;

namespace Sensus.Infrastructure.Persistence;

/// <summary>
/// Contexto principal de Entity Framework Core que implementa IUnitOfWork.
/// </summary>
public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Sensor> Sensors => Set<Sensor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // En un escenario con un bus o outbox, los eventos de dominio de las entidades
        // se recolectan aquí antes o después de SaveChangesAsync para su publicación garantizada.
        return await base.SaveChangesAsync(cancellationToken);
    }
}
