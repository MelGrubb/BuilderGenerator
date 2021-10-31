using System;

namespace BuilderGenerator.Test.Direct.Models.Entities
{
    [GenerateBuilder]
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
