#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using BuilderGenerator.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace BuilderGenerator;

// TODO: Revisit the Environment.NewLine problem. How can I get to the project setting?

[Generator]
internal class BuilderGenerator : IIncrementalGenerator
{
    private static readonly string BuilderBaseClass;
    private static readonly string BuilderClass;
    private static readonly string BuilderForAttribute;
    private static readonly string BuildMethod;
    private static readonly string BuildMethodSetter;
    private static readonly string Property;
    private static readonly string WithMethod;
    private static int _count;

    static BuilderGenerator()
    {
        var assembly = typeof(BuilderGenerator).Assembly;

        BuilderBaseClass = GetResourceAsString(assembly, nameof(BuilderBaseClass));
        BuilderClass = GetResourceAsString(assembly, nameof(BuilderClass));
        BuilderForAttribute = GetResourceAsString(assembly, nameof(BuilderForAttribute));
        BuildMethodSetter = GetResourceAsString(assembly, nameof(BuildMethodSetter));
        BuildMethod = GetResourceAsString(assembly, nameof(BuildMethod));
        Property = GetResourceAsString(assembly, nameof(Property));
        WithMethod = GetResourceAsString(assembly, nameof(WithMethod));
    }

    public static string GetResourceAsString(Assembly assembly, string resourceName)
    {
        resourceName = assembly.GetManifestResourceNames().Single(x => x.Equals($"BuilderGenerator.Templates.{resourceName}.txt", StringComparison.OrdinalIgnoreCase));

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

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Register injection of classes that never change.
        context.RegisterPostInitializationOutput(
            x =>
            {
                x.AddSource(nameof(BuilderBaseClass), SourceText.From(BuilderBaseClass, Encoding.UTF8));
                x.AddSource(nameof(BuilderForAttribute), SourceText.From(BuilderForAttribute, Encoding.UTF8));
            });

        // Register generation for classes based on the project contents
        var provider = context.SyntaxProvider.CreateSyntaxProvider(Predicate, Transform).Where(static builderInfo => builderInfo is not null).Collect().SelectMany((builders, _) => builders.Distinct());
        context.RegisterSourceOutput(provider, Execute);
    }

    private static void Execute(SourceProductionContext context, BuilderInfo? builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        var templateParser = new TemplateParser();

        try
        {
            templateParser.SetTag("Count", ++_count);
            templateParser.SetTag("GeneratedAt", DateTime.Now.ToString("s"));
            templateParser.SetTag("BuilderClassUsingBlock", builder.Value.BuilderClassUsingBlock);
            templateParser.SetTag("BuilderClassNamespace", builder.Value.BuilderClassNamespace);
            templateParser.SetTag("BuilderClassAccessibility", builder.Value.BuilderClassAccessibility);
            templateParser.SetTag("BuilderClassName", builder.Value.BuilderClassName);
            templateParser.SetTag("TargetClassName", builder.Value.TargetClassName);
            templateParser.SetTag("TargetClassFullName", builder.Value.TargetClassFullName);

            var properties = builder.Value.Properties;

            templateParser.SetTag("Properties", GenerateProperties(templateParser, properties));
            templateParser.SetTag("BuildMethod", GenerateBuildMethod(templateParser, properties));
            templateParser.SetTag("WithMethods", GenerateWithMethods(templateParser, properties));

            var source = templateParser.ParseString(BuilderClass);
            context.AddSource($"{builder.Value.BuilderClassName}.g.cs", SourceText.From(source, Encoding.UTF8));
        }
        catch (Exception e)
        {
            context.ReportDiagnostic(DiagnosticDescriptors.UnexpectedErrorDiagnostic(e, builder.Value.Location, builder.Value.Identifier));
        }
    }

    private static string GenerateBuildMethod(TemplateParser templateParser, IEnumerable<PropertyInfo> properties)
    {
        var setters = string.Join(
#pragma warning disable RS1035
            Environment.NewLine,
#pragma warning restore RS1035
            properties.Select(
                x =>
                {
                    templateParser.SetTag("PropertyName", x.Name);

                    return templateParser.ParseString(BuildMethodSetter);
                }));

        templateParser.SetTag("Setters", setters);
        var result = templateParser.ParseString(BuildMethod);

        return result;
    }

    private static string GenerateProperties(TemplateParser templateParser, IEnumerable<PropertyInfo> properties)
    {
        var result = string.Join(
#pragma warning disable RS1035
            Environment.NewLine,
#pragma warning restore RS1035
            properties.Select(
                x =>
                {
                    templateParser.SetTag("PropertyName", x.Name);
                    templateParser.SetTag("PropertyType", x.Type);

                    return templateParser.ParseString(Property);
                }));

        return result;
    }

    private static string GenerateWithMethods(TemplateParser templateParser, IEnumerable<PropertyInfo> properties)
    {
        var result = string.Join(
#pragma warning disable RS1035
            Environment.NewLine,
#pragma warning restore RS1035
            properties.Select(
                x =>
                {
                    templateParser.SetTag("PropertyName", x.Name);
                    templateParser.SetTag("PropertyType", x.Type);

                    return templateParser.ParseString(WithMethod);
                }));

        return result;
    }

    private static IEnumerable<IPropertySymbol> GetPropertySymbols(INamedTypeSymbol namedTypeSymbol, bool includeInternals)
    {
        var symbols = namedTypeSymbol.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.SetMethod is not null && (x.SetMethod.DeclaredAccessibility == Accessibility.Public || (includeInternals && x.SetMethod.DeclaredAccessibility == Accessibility.Internal)))
            .ToList();

        var baseTypeSymbol = namedTypeSymbol.BaseType;

        while (baseTypeSymbol != null)
        {
            symbols.AddRange(GetPropertySymbols(baseTypeSymbol, includeInternals));
            baseTypeSymbol = baseTypeSymbol.BaseType;
        }

        return symbols;
    }

    /// <summary>Performs a first-pass filtering of syntax nodes that might possibly represent a builder class.</summary>
    /// <param name="node">The syntax node being examined.</param>
    /// <param name="_">A cancellation token (currently unused).</param>
    /// <returns>A <see cref="bool" /> indicating whether or not <paramref name="node" /> might possibly represent a builder class.</returns>
    private static bool Predicate(SyntaxNode node, CancellationToken _) => node is TypeDeclarationSyntax { AttributeLists.Count: > 0 };

    /// <summary>Transforms the syntax node into a <see cref="BuilderInfo" /> containing the information needed to generate a Builder.</summary>
    /// <param name="context">The <see cref="GeneratorSyntaxContext" />, which contains a reference to the node.</param>
    /// <param name="token">A cancellation token, used to short-circuit the transformation if additional changes are detected.</param>
    /// <returns>A <see cref="BuilderInfo" /> describing the Builder if the node represents a builder; otherwise, null.</returns>
    private static BuilderInfo? Transform(GeneratorSyntaxContext context, CancellationToken token)
    {
        var node = context.Node;

        if (node is not TypeDeclarationSyntax typeNode) { return null; }

        var typeSymbol = context.SemanticModel.GetDeclaredSymbol(typeNode, token);

        if (typeSymbol is not INamedTypeSymbol namedTypeSymbol) { return null; }

        if (!namedTypeSymbol.GetAttributes().Any(x => x.AttributeClass?.Name == "BuilderForAttribute")) { return null; }

        var attributeSymbol = namedTypeSymbol.GetAttributes().SingleOrDefault(x => x.AttributeClass!.Name == nameof(BuilderForAttribute));

        if (attributeSymbol is null) { return null; }

        // The node represents a Builder class, so we can go ahead and do the transformation now.
        // We gather all the relevant information up-front so that it can be used effectively for caching.
        var targetClassType = attributeSymbol.ConstructorArguments[0];
        var includeInternals = (bool)attributeSymbol.ConstructorArguments[1].Value!;

        var targetClassProperties = GetPropertySymbols((INamedTypeSymbol)targetClassType.Value!, includeInternals)
            .Select<IPropertySymbol, (string Name, string TypeName, Accessibility Accessibility)>(x => new ValueTuple<string, string, Accessibility>(x.Name, x.Type.ToString(), x.DeclaredAccessibility))
            .Distinct()
            .OrderBy(x => x.Name)
            .ToList();

        var result = new BuilderInfo
        {
            // TODO: Store enum values instead of strings where possible
            BuilderClassAccessibility = typeSymbol.DeclaredAccessibility.ToString().ToLower(),
            BuilderClassNamespace = typeSymbol.ContainingNamespace.ToString(),
            BuilderClassName = typeSymbol.Name,
            TargetClassName = ((ISymbol)targetClassType.Value!).Name,
            TargetClassFullName = targetClassType.Value!.ToString(),
            BuilderClassUsingBlock = ((CompilationUnitSyntax)typeNode.SyntaxTree.GetRoot()).Usings.ToString(),
            Properties = targetClassProperties.Select(
                x => new PropertyInfo
                {
                    Name = x.Name,
                    Type = x.TypeName,
                    Accessibility = x.Accessibility,
                }).ToList(),
            Location = typeNode.GetLocation(),
            Identifier = typeNode.Identifier.ToString(),
        };

        return result;
    }
}
