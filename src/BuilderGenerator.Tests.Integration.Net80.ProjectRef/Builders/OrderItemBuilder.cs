using System;
using BuilderGenerator.Tests.Integration.Net80.ProjectRef.Models.Entities;

namespace BuilderGenerator.Tests.Integration.Net80.ProjectRef.Builders;

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
