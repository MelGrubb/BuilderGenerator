using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

// ReSharper disable InconsistentNaming

namespace BuilderGenerator.Tests.Unit;

public abstract class Given_a_BuilderGenerator
{
    protected static string GetResourceAsString(Assembly assembly, string resourceName)
    {
        var manifestResourceNames = assembly.GetManifestResourceNames();
        resourceName = manifestResourceNames.Single(x => x.Equals($"BuilderGenerator.Tests.Unit.Examples.{resourceName}", StringComparison.OrdinalIgnoreCase));

        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    protected static Compilation CreateCompilation(string source)
    {
        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(BuilderForAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0").Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a").Location),
        };

        var compilation = CSharpCompilation.Create("compilation", [CSharpSyntaxTree.ParseText(source)], references, new CSharpCompilationOptions(OutputKind.ConsoleApplication));

        return compilation;
    }
}
