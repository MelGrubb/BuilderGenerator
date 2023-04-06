using System;
using BuilderGenerator.Test.Net60.Models;

namespace BuilderGenerator.Test.Net60.Builders;

[BuilderFor(typeof(RecordExample))]
public partial class RecordExampleBuilder
{
    public static RecordExampleBuilder Typical()
    {
        return new RecordExampleBuilder()
            .WithParameterOne(Random.Shared.Next())
            .WithParameterTwo(Random.Shared.Next(0, 2) == 1)
            .WithParameterThree(Guid.NewGuid().ToString);
    }
}
