using System;

namespace BuilderGenerator.Test.Direct.Models.Entities
{
    public class OrderItem : Entity
    {
        public Guid ItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}