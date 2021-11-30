using System;
using BuilderGenerator.Test.Local.Models.Entities;

namespace BuilderGenerator.Test.Local.Builders
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
