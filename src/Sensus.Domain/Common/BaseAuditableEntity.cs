namespace Sensus.Domain.Common;

/// <summary>
/// Clase base para entidades que registran información de auditoría.
/// </summary>
public abstract class BaseAuditableEntity<TId> : BaseEntity<TId>
{
    public DateTime CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedAtUtc { get; set; }
    public string? LastModifiedBy { get; set; }
}

public abstract class BaseAuditableEntity : BaseAuditableEntity<Guid>
{
    protected BaseAuditableEntity()
    {
        Id = Guid.NewGuid();
    }

    protected BaseAuditableEntity(Guid id)
    {
        Id = id;
    }
}
