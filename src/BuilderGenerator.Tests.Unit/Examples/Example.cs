using System;
using System.Linq;

namespace BuilderGenerator.Tests.Unit.Examples;

/// <summary>Represents a basic business entity.</summary>
public abstract class Entity
{
    /// <summary>Uniquely identifies an Entity instance.</summary>
    public Guid Id { get; set; }
}

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

/// <summary>An example class with various properties.</summary>
public class Person : AuditableEntity
{
    /// <summary>Uniquely identifies a <see cref="Person"/> instance.</summary>
    public override Guid Id { get; set; }

    /// <summary>
    ///     The Person's first name.
    /// </summary>
    /// <remarks>This was a multi-line, indented summary in the original source code.</remarks>
    public string FirstName { get; set; }

    /// <summary>The Person's last name.</summary>
    /// <remarks>This was a single-line summary in the original source code.</remarks>
    public string LastName { get; set; }

    /// <summary>A string property marked as obsolete to test the "includeObsolete" attribute parameter.</summary>
    /// <remarks>This was a single-line summary in the original source code.</remarks>
    public string MiddleName { get; set; }

    /// <summary>A string with internal accessibility to test the "includeInternals" attribute parameter.</summary>
    /// <remarks>This was a single-line summary in the original source code.</remarks>
    internal string InternalString { get; set; }

    /// <summary>A string with internal accessibility to test the "includeInternals" attribute parameter.</summary>
    /// <remarks>This was a single-line summary in the original source code.</remarks>
    [Obsolete]
    public string ObsoleteString { get; set; }
}
