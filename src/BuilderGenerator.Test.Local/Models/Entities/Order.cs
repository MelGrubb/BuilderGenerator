using System;
using System.Collections.Generic;
using BuilderGenerator.Test.Local.Models.Enums;

namespace BuilderGenerator.Test.Local.Models.Entities
{
    public class Order : Entity
    {
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> Orders { get; set; } = new List<OrderItem>();
        public OrderStatus Status { get; set; }
    }
}
