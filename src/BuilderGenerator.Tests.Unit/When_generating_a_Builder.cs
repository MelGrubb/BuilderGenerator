using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Shouldly;
using VerifyTests;
using VerifyXunit;
using Xunit;

// ReSharper disable InconsistentNaming

namespace BuilderGenerator.Tests.Unit;

public class When_generating_a_Builder : Given_a_BuilderGenerator
{
    [Fact]
    public Task Test()
    {
        var assembly = GetType().Assembly;

        var inputCompilation = CreateCompilation(GetResourceAsString(assembly, "Example.cs"))
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(GetResourceAsString(assembly, "ExampleBuilder.cs")));

        var driver = CSharpGeneratorDriver.Create(new BuilderGenerator())
            .RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

        diagnostics.ShouldBeEmpty();
        outputCompilation.SyntaxTrees.Count().ShouldBe(3); // The entities, the builder stub, and the output

        var runResult = driver.GetRunResult();
        runResult.Diagnostics.ShouldBeEmpty();
        runResult.GeneratedTrees.Length.ShouldBe(1); // The generated builder

        var generatorResult = runResult.Results[0];
        generatorResult.Generator.GetGeneratorType().ShouldBe(typeof(BuilderGenerator));
        generatorResult.Exception.ShouldBeNull();
        generatorResult.GeneratedSources.Length.ShouldBe(1);

        var output = generatorResult.GeneratedSources[0].SourceText.ToString();
        var settings = new VerifySettings();
        settings.ScrubLinesWithReplace(x => x.StartsWith(TagLine) ? $"{TagLine}." : x);
        settings.UseDirectory("Verify");

        return Verifier.Verify(output, settings);
    }
}
