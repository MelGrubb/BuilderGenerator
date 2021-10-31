using System;
using System.Collections.Generic;
using BuilderGenerator.Tests.Models.Enums;

namespace BuilderGenerator.Tests.Models.Entities
{
    [GenerateBuilder]
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> Orders { get; set; } = new List<OrderItem>();

        public OrderStatus Status { get; set; }
    }
}
