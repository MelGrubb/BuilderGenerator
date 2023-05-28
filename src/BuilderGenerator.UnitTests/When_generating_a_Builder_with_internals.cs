using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Shouldly;

// ReSharper disable InconsistentNaming

namespace BuilderGenerator.Tests.Unit;

[TestFixture]
public class When_generating_a_Builder_with_internals : Given_a_BuilderGenerator
{
    [Test]
    public void SimpleGeneratorTest()
    {
        var assembly = GetType().Assembly;
        var inputCompilation = CreateCompilation(GetResourceAsString(assembly, "InputWithInternals.cs"));
        var expectedOutput = GetResourceAsString(assembly, "OutputWithInternals.cs");
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new BuilderGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
        diagnostics.ShouldBeEmpty();
        outputCompilation.SyntaxTrees.Count().ShouldBe(4);

        var runResult = driver.GetRunResult();
        runResult.Diagnostics.ShouldBeEmpty();
        runResult.GeneratedTrees.Length.ShouldBe(3); // The Builder base class, the BuilderFor attribute, and the generated builder

        // TODO: Check for the presence of the Builder base class.
        // TODO: Check for the presence of the BuilderForAttribute class.

        var generatorResult = runResult.Results[0];
        generatorResult.Generator.GetGeneratorType().ShouldBe(new BuilderGenerator().GetType());
        generatorResult.Exception.ShouldBeNull();
        generatorResult.GeneratedSources.Length.ShouldBe(3);

        var sourceText = generatorResult.GeneratedSources[2].SourceText.ToString();

        // Since the generation time will keep changing, we'll just compare everything after the first instance of the word "using".
        sourceText[sourceText.IndexOf("using", StringComparison.OrdinalIgnoreCase)..].ShouldBe(expectedOutput[expectedOutput.IndexOf("using", StringComparison.OrdinalIgnoreCase)..]);
    }
}
