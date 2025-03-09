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
        var inputCompilation = CreateCompilation(GetResourceAsString(assembly, "Example.cs"));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new BuilderGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
        diagnostics.ShouldBeEmpty();
        outputCompilation.SyntaxTrees.Count().ShouldBe(2); // The input and the output

        var runResult = driver.GetRunResult();
        runResult.Diagnostics.ShouldBeEmpty();
        runResult.GeneratedTrees.Length.ShouldBe(1); // The generated builder

        var generatorResult = runResult.Results[0];
        generatorResult.Generator.GetGeneratorType().ShouldBe(new BuilderGenerator().GetType());
        generatorResult.Exception.ShouldBeNull();
        generatorResult.GeneratedSources.Length.ShouldBe(1);

        var output = generatorResult.GeneratedSources[0].SourceText.ToString();
        var settings = new VerifySettings();
        settings.ScrubLinesContaining("This code was generated by BuilderGenerator");
        settings.UseDirectory("Verify");

        return Verifier.Verify(output, settings);
    }
}
