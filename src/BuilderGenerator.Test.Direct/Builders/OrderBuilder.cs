using System;
using BuilderGenerator.Test.Direct.Models.Entities;

namespace BuilderGenerator.Test.Direct.Builders
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
