using System;
using BuilderGenerator.Test.NuGet.Models.Entities;

namespace BuilderGenerator.Test.NuGet.Builders
{
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
}
