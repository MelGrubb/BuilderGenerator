using System;
using System.Collections.Generic;
using BuilderGenerator.Tests.Integration.Net80.PackageRef.Models.Entities;

namespace BuilderGenerator.Tests.Integration.Net80.PackageRef.Builders;

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
