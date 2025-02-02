using System;
using System.Collections.Generic;
using BuilderGenerator.Tests.Core.Models.Entities;

namespace BuilderGenerator.Tests.Integration.Net90.ProjectRef.Builders;

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
