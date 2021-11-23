using System;

namespace BuilderGenerator.Test.Direct.Models.Entities.Builders
{
    [BuilderFor(typeof(OrderItem))]
    public partial class OrderItemBuilder
    {
        public static OrderItemBuilder Simple()
        {
            var builder = new OrderItemBuilder()
                .WithId(Guid.NewGuid);

            return builder;
        }
    }
}
