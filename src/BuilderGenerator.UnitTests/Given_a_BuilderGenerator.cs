using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Shouldly;

namespace BuilderGenerator.UnitTests;

[TestFixture]
public class GeneratorTests
{
    [Test]
    public void SimpleGeneratorTest()
    {
        var assembly = GetType().Assembly;
        var inputCompilation = CreateCompilation(GetResourceAsString(assembly, "Input.cs"));
        var expectedOutput = GetResourceAsString(assembly, "Output.cs");
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new BuilderGenerator());
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
        diagnostics.ShouldBeEmpty();
        outputCompilation.SyntaxTrees.Count().ShouldBe(4);

        //outputCompilation.GetDiagnostics().ShouldBeEmpty();

        var runResult = driver.GetRunResult();
        runResult.Diagnostics.ShouldBeEmpty();

        runResult.GeneratedTrees.Length.ShouldBe(3); // The Builder base class, the BuilderFor attribute, and the generated builder.

        // TODO: Check for the presence of the Builder base class.
        // TODO: Check for the presence of the BuilderForAttribute class.

        var generatorResult = runResult.Results[0];
        generatorResult.Generator.GetGeneratorType().ShouldBe(new BuilderGenerator().GetType());
        generatorResult.Exception.ShouldBeNull();
        generatorResult.GeneratedSources.Length.ShouldBe(3);
        generatorResult.GeneratedSources[2].SourceText.ToString().ShouldBe(expectedOutput);
    }

    public static string GetResourceAsString(Assembly assembly, string resourceName)
    {
        var manifestResourceNames = assembly.GetManifestResourceNames();
        resourceName = manifestResourceNames.Single(x => x.Equals($"BuilderGenerator.UnitTests.Examples.{resourceName}", StringComparison.OrdinalIgnoreCase));

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
            {
                throw new InvalidOperationException($"Resource '{resourceName}' not found.");
            }

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }

    private static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create(
            "compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
}
