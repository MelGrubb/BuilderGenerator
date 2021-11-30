using System;
using BuilderGenerator.Test.Local.Models.Entities;

namespace BuilderGenerator.Test.Local.Builders
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
