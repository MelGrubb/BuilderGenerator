using System;

namespace BuilderGenerator.Test.Direct.Models.Entities.Builders
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
