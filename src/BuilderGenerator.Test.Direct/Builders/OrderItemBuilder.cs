using System;
using BuilderGenerator.Test.Direct.Models.Entities;

namespace BuilderGenerator.Test.Direct.Builders
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
