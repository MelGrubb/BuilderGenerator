using System;

namespace BuilderGenerator.Tests.Integration.Net80.ProjectRef.Models.Entities;

/// <summary>Represents a basic business entity.</summary>
public abstract class Entity
{
    /// <summary>Uniquely identifies an instance.</summary>
    public virtual Guid Id { get; set; }

    // The Builders should only expose this in builder classes where the attribute has enabled it
    internal string InternalString { get; set; } = null!;
}
