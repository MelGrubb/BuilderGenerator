using System;
using BuilderGenerator.Tests.Integration.Net70.ProjectRef.Models.Entities;

namespace BuilderGenerator.Tests.Integration.Net70.ProjectRef.Builders;

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
