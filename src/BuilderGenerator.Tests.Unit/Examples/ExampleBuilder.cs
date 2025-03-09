namespace BuilderGenerator.Tests.Unit.Examples;

/// <summary>An example builder that includes neither internal nor obsolete properties.</summary>
[BuilderFor(typeof(Person))]
public partial class PersonBuilder
{
}
