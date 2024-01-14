using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using BuilderGenerator.Diagnostics;
using BuilderGenerator.Templates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace BuilderGenerator;

[Generator]
internal class BuilderGenerator : IIncrementalGenerator
{
#pragma warning disable RS1035
    private static readonly string NewLine = Environment.NewLine; // TODO: Derive this value from the project itself
#pragma warning restore RS1035

    private static readonly CSharp Templates = new CSharp11();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Register injection of classes to be injected as-is.
        context.RegisterPostInitializationOutput(
            x =>
            {
                x.AddSource(nameof(Templates.BuilderBaseClass), SourceText.From(Templates.BuilderBaseClass, Encoding.UTF8));
                x.AddSource(nameof(Templates.BuilderForAttribute), SourceText.From(Templates.BuilderForAttribute, Encoding.UTF8));
            });

        // Register generation for classes based on the project contents
        var provider = context.SyntaxProvider.CreateSyntaxProvider(Predicate, Transform).Where(static builderInfo => builderInfo is not null).Collect().SelectMany((builders, _) => builders.Distinct());
        context.RegisterSourceOutput(provider, Execute);
    }

    private static void Execute(SourceProductionContext context, BuilderInfo? builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        var stopwatch = Stopwatch.StartNew();
        var templateParser = new TemplateParser();

        try
        {
            templateParser.SetTag("GenerationTime", DateTime.Now.ToString("s"));
            templateParser.SetTag("GenerationDuration", (builder.Value.TimeToGenerate + stopwatch.Elapsed).TotalMilliseconds);
            templateParser.SetTag("BuilderClassUsingBlock", builder.Value.BuilderClassUsingBlock);
            templateParser.SetTag("BuilderClassNamespace", builder.Value.BuilderClassNamespace);
            templateParser.SetTag("BuilderClassAccessibility", builder.Value.BuilderClassAccessibility.ToString().ToLower());
            templateParser.SetTag("BuilderClassName", builder.Value.BuilderClassName);
            templateParser.SetTag("TargetClassName", builder.Value.TargetClassName);
            templateParser.SetTag("TargetClassFullName", builder.Value.TargetClassFullName);
            templateParser.SetTag("Constructors", GenerateConstructors(templateParser));
            templateParser.SetTag("Properties", GenerateProperties(templateParser, builder.Value.Properties));
            templateParser.SetTag("WithValuesFromMethod", GenerateWithValuesFromMethod(templateParser, builder.Value.Properties));
            templateParser.SetTag("BuildMethod", GenerateBuildMethod(templateParser, builder.Value.Properties));
            templateParser.SetTag("WithMethods", GenerateWithMethods(templateParser, builder.Value.Properties));
            templateParser.SetTag("WithObjectMethod", GenerateWithObjectMethod(templateParser));
            var source = templateParser.ParseString(Templates.BuilderClass);
            context.AddSource($"{builder.Value.BuilderClassName}.g.cs", SourceText.From(source, Encoding.UTF8));
        }
        catch (Exception e)
        {
            context.ReportDiagnostic(DiagnosticDescriptors.UnexpectedErrorDiagnostic(e, builder.Value.Location, builder.Value.Identifier));
        }
    }

    private static string GenerateBuildMethod(TemplateParser templateParser, IEnumerable<BuilderInfo.PropertyInfo> properties)
    {
        var setters = string.Join(
            NewLine,
            properties.Select(
                p =>
                {
                    templateParser.SetTag("PropertyName", p.Name);

                    return templateParser.ParseString(Templates.BuildMethodSetter);
                }));

        templateParser.SetTag("Setters", setters);
        var result = templateParser.ParseString(Templates.BuildMethod);

        return result;
    }

    private static string GenerateConstructors(TemplateParser templateParser)
    {
        return templateParser.ParseString(Templates.Constructors);
    }

    private static string GenerateProperties(TemplateParser templateParser, IEnumerable<BuilderInfo.PropertyInfo> properties)
    {
        var result = string.Join(
            NewLine,
            properties.Select(
                p =>
                {
                    templateParser.SetTag("PropertyName", p.Name);
                    templateParser.SetTag("PropertyType", p.Type);

                    return templateParser.ParseString(Templates.Property);
                }));

        return result;
    }

    private static string GenerateWithMethods(TemplateParser templateParser, IEnumerable<BuilderInfo.PropertyInfo> properties)
    {
        var result = string.Join(
            NewLine,
            properties.Select(
                p =>
                {
                    templateParser.SetTag("PropertyName", p.Name);
                    templateParser.SetTag("PropertyType", p.Type);

                    return templateParser.ParseString(Templates.WithMethods);
                }));

        return result;
    }

    private static string GenerateWithObjectMethod(TemplateParser templateParser)
    {
        return templateParser.ParseString(Templates.WithObjectMethod);
    }

    private static string GenerateWithValuesFromMethod(TemplateParser templateParser, IEnumerable<BuilderInfo.PropertyInfo> properties)
    {
        var setters = string.Join(
            NewLine,
            properties.Select(
                p =>
                {
                    templateParser.SetTag("PropertyName", p.Name);

                    return templateParser.ParseString(Templates.WithValuesFromSetter);
                }));

        templateParser.SetTag("WithValuesFromSetters", setters);
        var result = templateParser.ParseString(Templates.WithValuesFromMethod);

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

    /// <summary>Performs a first-pass filtering of syntax nodes that could possibly represent a builder class.</summary>
    /// <param name="node">The syntax node being examined.</param>
    /// <param name="_">A cancellation token (currently unused).</param>
    /// <returns>A <see cref="bool" /> indicating whether <paramref name="node" /> might possibly represent a builder class.</returns>
    private static bool Predicate(SyntaxNode node, CancellationToken _)
    {
        return node is TypeDeclarationSyntax { AttributeLists.Count: > 0 };
    }

    /// <summary>Transforms the syntax node into a <see cref="BuilderInfo" /> containing the information needed to generate a Builder.</summary>
    /// <param name="context">The <see cref="GeneratorSyntaxContext" />, which contains a reference to the node.</param>
    /// <param name="token">A cancellation token, used to short-circuit the transformation if additional changes are detected.</param>
    /// <returns>A <see cref="BuilderInfo" /> describing the Builder if the node represents a builder; otherwise, null.</returns>
    private static BuilderInfo? Transform(GeneratorSyntaxContext context, CancellationToken token)
    {
        var stopwatch = Stopwatch.StartNew();
        var node = context.Node;

        if (node is not TypeDeclarationSyntax typeNode) { return null; }

        var typeSymbol = context.SemanticModel.GetDeclaredSymbol(typeNode, token);

        if (typeSymbol is not INamedTypeSymbol namedTypeSymbol) { return null; }

        if (!namedTypeSymbol.GetAttributes().Any(x => x.AttributeClass?.Name == "BuilderForAttribute")) { return null; }

        var attributeSymbol = namedTypeSymbol.GetAttributes().SingleOrDefault(x => x.AttributeClass!.Name == nameof(Templates.BuilderForAttribute));

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
            BuilderClassAccessibility = typeSymbol.DeclaredAccessibility,
            BuilderClassNamespace = typeSymbol.ContainingNamespace.ToString(),
            BuilderClassName = typeSymbol.Name,
            TargetClassName = ((ISymbol)targetClassType.Value!).Name,
            TargetClassFullName = targetClassType.Value!.ToString(),
            BuilderClassUsingBlock = ((CompilationUnitSyntax)typeNode.SyntaxTree.GetRoot()).Usings.ToString(),
            Properties = targetClassProperties.Select(
                x => new BuilderInfo.PropertyInfo
                {
                    Accessibility = x.Accessibility,
                    Name = x.Name,
                    Type = x.TypeName,
                }).ToList(),
            Location = typeNode.GetLocation(),
            Identifier = typeNode.Identifier.ToString(),

            // TODO: Retrieve template class instance/type from attribute, and use that to retrieve the strings later on.
            TimeToGenerate = stopwatch.Elapsed,
        };

        return result;
    }
}
