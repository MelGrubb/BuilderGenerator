using System;
using BuilderGenerator.Test.Core.Models.Entities;

namespace BuilderGenerator.Test.Net60.Builders;

[BuilderFor(typeof(Order))]
public partial class OrderBuilder
{
    public static OrderBuilder Simple()
    {
        var builder = new OrderBuilder()
            .WithId(Guid.NewGuid);

        return builder;
    }
}
