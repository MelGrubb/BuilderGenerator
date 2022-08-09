using System;
using System.Collections.Generic;
using BuilderGenerator.Test.Core.Models.Entities;

namespace BuilderGenerator.Test.Net50.Builders;

[BuilderFor(typeof(Order), true)]
public partial class OrderBuilder
{
    public static OrderBuilder Simple()
    {
        var builder = new OrderBuilder()
            .WithId(Guid.NewGuid);

        return builder;
    }

    public static OrderBuilder Typical()
    {
        var builder = Simple()
            .WithItems(
                () => new List<OrderItem>
                {
                    OrderItemBuilder.Simple().Build(),
                });

        return builder;
    }
}
