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
        // Create the 'input' compilation that the generator will act on
        var inputCompilation = CreateCompilation(
            @"
namespace MyCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}
");

        // directly create an instance of the generator
        // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
        var generator = new BuilderGenerator();

        // Create the driver that will control the generation, passing in our generator
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Run the generation pass
        // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

        // We can now assert things about the resulting compilation:
        diagnostics.ShouldBeEmpty(); // there were no diagnostics created by the generators
        outputCompilation.SyntaxTrees.Count().ShouldBe(3); // we have two syntax trees, the original 'user' provided one, and the one added by the generator
        outputCompilation.GetDiagnostics().ShouldBeEmpty(); // verify the compilation with the added source has no diagnostics

        // Or we can look at the results directly:
        var runResult = driver.GetRunResult();

        // The runResult contains the combined results of all generators passed to the driver
        runResult.GeneratedTrees.Length.ShouldBe(2);
        runResult.Diagnostics.ShouldBeEmpty();

        // TODO: Check for the presence of the Builder base class.
        // TODO: Check for the presence of the BuilderForAttribute class.

        // Or you can access the individual results on a by-generator basis
        //var generatorResult = runResult.Results[0];
        //Debug.Assert(generatorResult.Generator == generator);
        //Debug.Assert(generatorResult.Diagnostics.IsEmpty);
        //Debug.Assert(generatorResult.GeneratedSources.Length == 1);
        //Debug.Assert(generatorResult.Exception is null);
    }

    private static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create(
            "compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
}
