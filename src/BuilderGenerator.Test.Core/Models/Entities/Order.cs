using System;
using System.Collections.Generic;
using BuilderGenerator.Test.Core.Models.Enums;

namespace BuilderGenerator.Test.Core.Models.Entities;

public class Order : AuditableEntity
{
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
}
