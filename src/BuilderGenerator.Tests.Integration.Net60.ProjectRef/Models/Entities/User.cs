using System;
using System.Collections.Generic;

namespace BuilderGenerator.Tests.Integration.Net60.ProjectRef.Models.Entities;

/// <summary>Represents an individual user of the system.</summary>
public class User : AuditableEntity
{
    /// <summary>Uniquely identifies a <see cref="User"/>.</summary>
    public override Guid Id { get; set; }

    /// <summary>The <see cref="User" />'s first name.</summary>
    public string FirstName { get; set; } = null!;

    /// <summary>The <see cref="User" />'s last name.</summary>
    public string LastName { get; set; } = null!;

    /// <summary>The <see cref="User" />'s middle name.</summary>
    public string? MiddleName { get; set; }

    /// <summary>The <see cref="User" />'s <see cref="Order" />s.</summary>
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
