using System;
using BuilderGenerator.Test.NuGet.Models.Entities;

namespace BuilderGenerator.Test.NuGet.Builders
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
