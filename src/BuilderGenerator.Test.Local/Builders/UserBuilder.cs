using System;
using System.Collections.Generic;
using BuilderGenerator.Test.Local.Models.Entities;

namespace BuilderGenerator.Test.Local.Builders
{
    [BuilderFor(typeof(User))]
    public partial class UserBuilder
    {
        public static UserBuilder Simple()
        {
            var builder = new UserBuilder()
                .WithId(Guid.NewGuid)
                .WithFirstName(() => Guid.NewGuid().ToString())
                .WithMiddleName(() => Guid.NewGuid().ToString())
                .WithLastName(() => Guid.NewGuid().ToString());

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
