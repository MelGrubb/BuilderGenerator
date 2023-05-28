using System;
using BuilderGenerator.Tests.Core.Models.Entities;

namespace BuilderGenerator.Tests.Integration.Net60.Builders;

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
