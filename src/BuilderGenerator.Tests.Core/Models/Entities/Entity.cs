using System;

namespace BuilderGenerator.Tests.Core.Models.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }

    // The Builders should only expose this in builder classes where the attribute has enabled it
    internal string InternalString { get; set; } = null!;
}
