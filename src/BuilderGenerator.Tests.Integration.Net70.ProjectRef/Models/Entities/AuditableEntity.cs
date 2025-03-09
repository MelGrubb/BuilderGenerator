using System;

namespace BuilderGenerator.Tests.Integration.Net70.ProjectRef.Models.Entities;

/// <summary>Represents entities with basic audit fields.</summary>
public abstract class AuditableEntity : Entity
{
    /// <summary>When this instance was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Who created this instance.</summary>
    public string CreatedBy { get; set; } = null!;

    /// <summary>When this instance was last updated.</summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>Who last updated this instance.</summary>
    public string UpdatedBy { get; set; } = null!;
}
