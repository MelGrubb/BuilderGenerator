using System;
using BuilderGenerator.Demo.Builders;

namespace BuilderGenerator.Demo
{
    public class FooTests
    {
        public FooTests()
        {
            new FooBuilder()
                .WithId(Guid.NewGuid)
                .WithFirstName("Bob")
                .WithLastName("Smith");
        }
    }
}
