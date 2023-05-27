using System;
using BuilderGenerator.IntegrationTests.Core.Models.Entities;

namespace BuilderGenerator.IntegrationTests.Net60.Builders;

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
