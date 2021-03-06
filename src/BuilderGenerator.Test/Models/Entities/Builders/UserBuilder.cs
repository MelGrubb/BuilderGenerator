using System;
using System.Collections.Generic;
using BuilderGenerator.Tests.Framework;

namespace BuilderGenerator.Tests.Models.Entities.Builders
{
    public partial class UserBuilder
    {
        public static UserBuilder Simple()
        {
            var builder = new UserBuilder()
                .WithId(Guid.NewGuid)
                .WithFirstName(() => GetRandom.FirstName())
                .WithLastName(() => GetRandom.LastName());

            return builder;
        }

        public static UserBuilder Typical()
        {
            var builder = Simple()
                .WithOrders(
                    () => new List<Order>
                    {
                        OrderBuilder.Simple().Build(),
                    });

            return builder;
        }
    }
}
