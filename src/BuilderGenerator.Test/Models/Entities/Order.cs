using System;
using System.Collections.Generic;
using BuilderGenerator.Test.Models.Enums;

namespace BuilderGenerator.Test.Models.Entities
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
