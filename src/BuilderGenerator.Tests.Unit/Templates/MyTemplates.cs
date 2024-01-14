using System;
using BuilderGenerator.Templates;

namespace BuilderGenerator.Tests.Unit.Templates;

/// <summary>Defines custom code generation templates for testing.</summary>
/// <remarks>This is here to test the custom template mechanism itself by overriding individual pieces.</remarks>
internal class MyTemplates : CSharp11
{
    // TODO: Override the BuilderClass template, changing the header to remove the timestamp only so that the results match.
    public override string BuilderClass
    {
        get
        {
            return base.BuilderClass[base.BuilderClass.IndexOf("using", StringComparison.OrdinalIgnoreCase)..];
        }
    }
}
