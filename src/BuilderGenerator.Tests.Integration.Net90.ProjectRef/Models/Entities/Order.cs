using System;
using System.Collections.Generic;
using OrderStatus = BuilderGenerator.Tests.Integration.Net90.ProjectRef.Models.Enums.OrderStatus;

namespace BuilderGenerator.Tests.Integration.Net90.ProjectRef.Models.Entities;

/// <summary>Represents a single order.</summary>
public class Order : AuditableEntity
{
    /// <summary>The <see cref="OrderItem" />s in this <see cref="Order" />.</summary>
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    /// <summary>When this <see cref="Order" /> was placed.</summary>
    public DateTime OrderDate { get; set; }

    /// <summary>A collection of <see cref="OrderItem" />s in this <see cref="Order" />.</summary>
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    /// <summary>This <see cref="Order" />'s current <see cref="OrderStatus" />.</summary>
    public OrderStatus Status { get; set; }
}
