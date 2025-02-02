using System;
using System.Collections.Generic;
using OrderStatus = BuilderGenerator.Tests.Integration.Net60.PackageRef.Models.Enums.OrderStatus;

namespace BuilderGenerator.Tests.Integration.Net60.PackageRef.Models.Entities;

public class Order : Integration.Net60.PackageRef.Models.Entities.AuditableEntity
{
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> Orders { get; set; } = new List<OrderItem>();
    public OrderStatus Status { get; set; }
}
