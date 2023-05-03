using System;
using System.Collections.Generic;
using BuilderGenerator.IntegrationTests.Core.Models.Enums;

namespace BuilderGenerator.IntegrationTests.Core.Models.Entities;

public class Order : AuditableEntity
{
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> Orders { get; set; } = new List<OrderItem>();
    public OrderStatus Status { get; set; }
}
