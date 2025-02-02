using System;
using BuilderGenerator.Tests.Integration.Net80.PackageRef.Models.Entities;

namespace BuilderGenerator.Tests.Integration.Net80.PackageRef.Builders;

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
