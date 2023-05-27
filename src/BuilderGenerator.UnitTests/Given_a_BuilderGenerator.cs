using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

// ReSharper disable InconsistentNaming

namespace BuilderGenerator.UnitTests;

public abstract class Given_a_BuilderGenerator
{
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

    protected static Compilation CreateCompilation(string source)
    {
        return CSharpCompilation.Create(
            "compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }
}
