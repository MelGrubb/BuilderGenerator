using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Shouldly;
using VerifyNUnit;
using VerifyTests;

// ReSharper disable InconsistentNaming

namespace BuilderGenerator.Tests.Unit;

[TestFixture]
public class When_generating_a_Builder_including_internals : Given_a_BuilderGenerator
{
    [Test]
    public Task SimpleGeneratorTest()
    {
        var assembly = GetType().Assembly;
        var inputCompilation = CreateCompilation(GetResourceAsString(assembly, "ExampleWithObsoleteProperties.cs"));
        var generator = new BuilderGenerator();
        var driver = CSharpGeneratorDriver.Create(generator).RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
        diagnostics.ShouldBeEmpty();
        outputCompilation.SyntaxTrees.Count().ShouldBe(2); // The input and three outputs

        var runResult = driver.GetRunResult();
        runResult.Diagnostics.ShouldBeEmpty();
        runResult.GeneratedTrees.Length.ShouldBe(1); // The generated builders

        var generatorResult = runResult.Results[0];
        generatorResult.Generator.GetGeneratorType().ShouldBe(generator.GetType());
        generatorResult.Exception.ShouldBeNull();
        generatorResult.GeneratedSources.Length.ShouldBe(1);

        var output = generatorResult.GeneratedSources[0].SourceText.ToString();
        var settings = new VerifySettings();
        settings.ScrubLinesContaining("This code was generated by BuilderGenerator");
        settings.UseDirectory("Verify");

        return Verifier.Verify(output, settings);
    }
}
