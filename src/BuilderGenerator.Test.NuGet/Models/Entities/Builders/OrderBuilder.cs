using System;

namespace BuilderGenerator.Test.NuGet.Models.Entities.Builders
{
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
