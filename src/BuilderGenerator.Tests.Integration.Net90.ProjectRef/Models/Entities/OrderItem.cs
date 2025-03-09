using System;

namespace BuilderGenerator.Tests.Integration.Net90.ProjectRef.Models.Entities;

/// <summary>Represents a single item in an <see cref="Order" />.</summary>
public class OrderItem : AuditableEntity
{
    /// <summary>The price quoted for the Product on this <see cref="Order" />.</summary>
    public decimal Price { get; set; }

    /// <summary>Identifies the Product being ordered.</summary>
    public Guid ProductId { get; set; }

    /// <summary>Indicates how many of the <see cref="Product" /> are being purchased.</summary>
    public int Quantity { get; set; }
}
