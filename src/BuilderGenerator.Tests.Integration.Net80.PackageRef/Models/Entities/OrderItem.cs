using System;

namespace BuilderGenerator.Tests.Integration.Net80.PackageRef.Models.Entities;

public class OrderItem : AuditableEntity
{
    public Guid ItemId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
