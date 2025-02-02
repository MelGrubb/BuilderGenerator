using System;

namespace BuilderGenerator.Tests.Integration.Net60.PackageRef.Models.Entities;

public class OrderItem : Integration.Net60.PackageRef.Models.Entities.AuditableEntity
{
    public Guid ItemId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
