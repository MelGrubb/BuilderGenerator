using System;
using System.Collections.Generic;
using BuilderGenerator.Test.Core.Models.Enums;

namespace BuilderGenerator.Test.Core.Models.Entities
{
    public class Order : AuditableEntity
    {
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> Orders { get; set; } = new List<OrderItem>();
        public OrderStatus Status { get; set; }
    }
}
