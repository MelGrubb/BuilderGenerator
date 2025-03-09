namespace BuilderGenerator.Tests.Unit.Examples;

/// <summary>An example builder that includes obsolete properties.</summary>
[BuilderFor(typeof(Person), includeObsolete: true)]
public partial class PersonBuilderWithObsolete
{
}
