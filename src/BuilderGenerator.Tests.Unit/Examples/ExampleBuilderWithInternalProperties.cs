namespace BuilderGenerator.Tests.Unit.Examples;

/// <summary>An example builder that includes neither internal nor obsolete properties.</summary>
[BuilderFor(typeof(Person), includeInternals: true)]
public partial class PersonBuilder
{
}
