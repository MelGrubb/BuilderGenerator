using System;

namespace BuilderGenerator.Tests.Core.Models.Entities;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = null!;
}
