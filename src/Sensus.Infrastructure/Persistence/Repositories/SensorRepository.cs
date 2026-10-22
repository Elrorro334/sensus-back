using Microsoft.EntityFrameworkCore;
using Sensus.Domain.Entities;
using Sensus.Domain.Enums;
using Sensus.Domain.Repositories;

namespace Sensus.Infrastructure.Persistence.Repositories;

public sealed class SensorRepository : ISensorRepository
{
    private readonly ApplicationDbContext _context;

    public SensorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Sensor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sensors
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Sensor?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();
        return await _context.Sensors
            .FirstOrDefaultAsync(s => s.Code == normalizedCode, cancellationToken);
    }

    public async Task<IReadOnlyList<Sensor>> GetAllAsync(
        string? searchTerm = null,
        SensorStatus? status = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Sensors.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(s =>
                s.Name.ToLower().Contains(term) ||
                s.Code.ToLower().Contains(term) ||
                s.Description.ToLower().Contains(term));
        }

        if (status.HasValue)
        {
            query = query.Where(s => s.Status == status.Value);
        }

        return await query
            .OrderByDescending(s => s.CreatedAtUtc)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        string? searchTerm = null,
        SensorStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Sensors.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(s =>
                s.Name.ToLower().Contains(term) ||
                s.Code.ToLower().Contains(term) ||
                s.Description.ToLower().Contains(term));
        }

        if (status.HasValue)
        {
            query = query.Where(s => s.Status == status.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();
        return await _context.Sensors
            .AnyAsync(s => s.Code == normalizedCode, cancellationToken);
    }

    public async Task AddAsync(Sensor sensor, CancellationToken cancellationToken = default)
    {
        await _context.Sensors.AddAsync(sensor, cancellationToken);
    }

    public void Update(Sensor sensor)
    {
        _context.Sensors.Update(sensor);
    }

    public void Delete(Sensor sensor)
    {
        _context.Sensors.Remove(sensor);
    }
}
