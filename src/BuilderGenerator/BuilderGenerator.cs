using System;
using System.Collections.Generic;
using System.Diagnostics;
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

[Generator]
internal class BuilderGenerator : IIncrementalGenerator
{
#pragma warning disable RS1035
    private static readonly string NewLine = Environment.NewLine; // TODO: Derive this value from the project itself
#pragma warning restore RS1035

    private static readonly string BuilderClass;
    private static readonly string BuilderProperty;
    private static readonly string BuildMethod;
    private static readonly string BuildMethodSetter;
    private static readonly string WithMethods;
    private static readonly string WithObjectMethod;
    private static readonly string WithObjectMethodSetter;
    private static readonly string WithValuesFromMethod;
    private static readonly string WithValuesFromSetter;

    static BuilderGenerator()
    {
        var assembly = typeof(BuilderGenerator).Assembly;

        BuilderClass = GetResourceAsString(assembly, $"{nameof(BuilderClass)}.cs");
        BuilderProperty = GetResourceAsString(assembly, $"{nameof(BuilderProperty)}.cs");
        BuildMethodSetter = GetResourceAsString(assembly, $"{nameof(BuildMethodSetter)}.cs");
        BuildMethod = GetResourceAsString(assembly, $"{nameof(BuildMethod)}.cs");
        WithMethods = GetResourceAsString(assembly, $"{nameof(WithMethods)}.cs");
        WithObjectMethod = GetResourceAsString(assembly, $"{nameof(WithObjectMethod)}.cs");
        WithObjectMethodSetter = GetResourceAsString(assembly, $"{nameof(WithObjectMethodSetter)}.cs");
        WithValuesFromMethod = GetResourceAsString(assembly, $"{nameof(WithValuesFromMethod)}.cs");
        WithValuesFromSetter = GetResourceAsString(assembly, $"{nameof(WithValuesFromSetter)}.cs");
    }

    private static string GetResourceAsString(Assembly assembly, string resourceName)
    {
        resourceName = assembly.GetManifestResourceNames().Single(x => x.Equals($"BuilderGenerator.Templates.{resourceName}", StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
        {
            throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        }

        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.ForAttributeWithMetadataName("BuilderGenerator.BuilderForAttribute", Predicate, Transform);
        context.RegisterSourceOutput(provider, Generate);
    }

    private static void Generate(SourceProductionContext context, BuilderInfo? builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        var stopwatch = Stopwatch.StartNew();
        var templateParser = new TemplateParser();

        try
        {
#if DEBUG
            // Only update the header in debug builds so we can tell when generation has been triggered.
            // Don't include it in release builds though, or it will cause unnecessary churn in the consuming project's repo.
            templateParser.SetTag("GenerationTime", $" at {DateTime.Now:s}");
            templateParser.SetTag("GenerationDuration", $" in {(builder.Value.TimeToGenerate + stopwatch.Elapsed).TotalMilliseconds}ms");
#endif
            templateParser.SetTag("BuilderClassUsingBlock", builder.Value.BuilderClassUsingBlock);
            templateParser.SetTag("BuilderClassNamespace", builder.Value.BuilderClassNamespace);
            templateParser.SetTag("BuilderClassAccessibility", builder.Value.BuilderClassAccessibility.ToString().ToLower());
            templateParser.SetTag("BuilderClassName", builder.Value.BuilderClassName);
            templateParser.SetTag("TargetClassName", builder.Value.TargetClassName);
            templateParser.SetTag("TargetClassFullName", builder.Value.TargetClassFullName);
            templateParser.SetTag("Properties", GenerateProperties(templateParser, builder.Value.Properties));
            templateParser.SetTag("WithValuesFromMethod", GenerateWithValuesFromMethod(templateParser, builder.Value.Properties));
            templateParser.SetTag(nameof(BuildMethod), GenerateBuildMethod(templateParser, builder.Value.Properties));
            templateParser.SetTag(nameof(WithMethods), GenerateWithMethods(templateParser, builder.Value.Properties));
            templateParser.SetTag(nameof(WithObjectMethod), GenerateWithObjectMethod(templateParser));

            var source = templateParser.ParseString(BuilderClass);
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
                    // Extract XML documentation comment for the property
                    var propertyComment = string.IsNullOrWhiteSpace(p.Comment)
                        ? $"{NewLine}<summary>With {p.Name}.</summary>" // Default if no comment is provided
                        : p.Comment;

                    templateParser.SetTag("PropertyComment", FormatXmlComments(propertyComment));
                    templateParser.SetTag("PropertyName", p.Name);

                    return templateParser.ParseString(BuildMethodSetter);
                }));

        templateParser.SetTag("Setters", setters);
        var result = templateParser.ParseString(BuildMethod);

        return result;
    }

    private static string GenerateProperties(TemplateParser templateParser, IEnumerable<BuilderInfo.PropertyInfo> properties)
    {
        var result = string.Join(
            NewLine,
            properties.Select(
                p =>
                {
                    // Extract XML documentation comment for the property
                    var propertyComment = string.IsNullOrWhiteSpace(p.Comment)
                        ? $"<summary>With {p.Name}.</summary>" // Default if no comment is provided
                        : p.Comment;

                    templateParser.SetTag("PropertyComment", FormatXmlComments(propertyComment));
                    templateParser.SetTag("PropertyName", p.Name);
                    templateParser.SetTag("PropertyType", p.Type);

                    return templateParser.ParseString(BuilderProperty);
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
                    // Extract XML documentation comment for the property
                    var propertyComment = string.IsNullOrWhiteSpace(p.Comment)
                        ? $"<summary>With {p.Name}.</summary>" // Default if no comment is provided
                        : p.Comment;

                    templateParser.SetTag("PropertyComment", FormatXmlComments(propertyComment));
                    templateParser.SetTag("PropertyName", p.Name);
                    templateParser.SetTag("PropertyType", p.Type);

                    return templateParser.ParseString(WithMethods);
                }));

        return result;
    }

    private static string GenerateWithObjectMethod(TemplateParser templateParser)
    {
        return templateParser.ParseString(WithObjectMethod);
    }

    private static string GenerateWithValuesFromMethod(TemplateParser templateParser, IEnumerable<BuilderInfo.PropertyInfo> properties)
    {
        var setters = string.Join(
            NewLine,
            properties.Select(
                p =>
                {
                    templateParser.SetTag("PropertyName", p.Name);

                    return templateParser.ParseString(WithValuesFromSetter);
                }));

        templateParser.SetTag("WithValuesFromSetters", setters);
        var result = templateParser.ParseString(WithValuesFromMethod);

        return result;
    }

    private static IEnumerable<IPropertySymbol> GetPropertySymbols(INamedTypeSymbol namedTypeSymbol, bool includeInternals, bool includeObsolete)
    {
        var baseTypeSymbol = namedTypeSymbol.BaseType;

        var symbols = namedTypeSymbol.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(
                x => x.SetMethod is not null
                    && (includeObsolete || !x.GetAttributes().Any(a => a.AttributeClass?.Name is "Obsolete" or "ObsoleteAttribute"))
                    && (x.SetMethod.DeclaredAccessibility == Accessibility.Public || (includeInternals && x.SetMethod.DeclaredAccessibility == Accessibility.Internal)))
            .ToList();

        while (baseTypeSymbol != null)
        {
            symbols.AddRange(GetPropertySymbols(baseTypeSymbol, includeInternals, includeObsolete));
            baseTypeSymbol = baseTypeSymbol.BaseType;
        }

        return symbols;
    }

    /// <summary>Performs a first-pass filtering of syntax nodes that could possibly represent a builder class.</summary>
    /// <param name="node">The syntax node being examined.</param>
    /// <param name="_">A cancellation token (currently unused).</param>
    /// <returns>A <see cref="bool" /> indicating whether <paramref name="node" /> may represent a builder class.</returns>
    private static bool Predicate(SyntaxNode node, CancellationToken _)
    {
        return true;
    }

    /// <summary>Transforms the syntax node into a <see cref="BuilderInfo" /> containing the information needed to generate a Builder.</summary>
    /// <param name="syntaxContext">The <see cref="GeneratorAttributeSyntaxContext" />, which contains a reference to the node.</param>
    /// <param name="token">A cancellation token, used to short-circuit the transformation if additional changes are detected.</param>
    /// <returns>A <see cref="BuilderInfo" /> describing the Builder if the node represents a builder; otherwise, null.</returns>
    private static BuilderInfo? Transform(GeneratorAttributeSyntaxContext syntaxContext, CancellationToken token)
    {
        var stopwatch = Stopwatch.StartNew();

        if (syntaxContext.TargetNode is not TypeDeclarationSyntax typeNode) { return null; }

        if (syntaxContext.SemanticModel.GetDeclaredSymbol(syntaxContext.TargetNode, token) is not INamedTypeSymbol namedTypeSymbol) { return null; }

        var attributes = namedTypeSymbol.GetAttributes();
        var attribute = attributes.SingleOrDefault(x => x.AttributeClass?.Name == "BuilderForAttribute");

        if (attribute == null) { return null; }

        // The node represents a Builder class, so we can go ahead and do the transformation now.
        // We gather all the relevant information up-front so that it can be used effectively for caching.
        var arguments = attribute.ConstructorArguments;
        var targetClassType = arguments[0];
        var includeInternals = arguments.Length > 1 && (bool)arguments[1].Value!;
        var includeObsolete = arguments.Length > 2 && (bool)arguments[2].Value!;

        var targetClassProperties = GetPropertySymbols((INamedTypeSymbol)targetClassType.Value!, includeInternals, includeObsolete)
            .Select<IPropertySymbol, (string Name, string TypeName, Accessibility Accessibility, string Comment)>(x => new ValueTuple<string, string, Accessibility, string>(x.Name, x.Type.ToString(), x.DeclaredAccessibility, x.GetDocumentationCommentXml()))
            .Distinct()
            .OrderBy(x => x.Name)
            .ToList();

        var result = new BuilderInfo
        {
            BuilderClassAccessibility = namedTypeSymbol.DeclaredAccessibility,
            BuilderClassNamespace = namedTypeSymbol.ContainingNamespace.ToString(),
            BuilderClassName = namedTypeSymbol.Name,
            TargetClassName = ((ISymbol)targetClassType.Value!).Name,
            TargetClassFullName = targetClassType.Value!.ToString(),
            BuilderClassUsingBlock = ((CompilationUnitSyntax)typeNode.SyntaxTree.GetRoot()).Usings.ToString(),
            Properties = targetClassProperties.Select(
                x => new BuilderInfo.PropertyInfo
                {
                    Accessibility = x.Accessibility,
                    Comment = x.Comment,
                    Name = x.Name,
                    Type = x.TypeName,
                }).ToList(),
            Location = typeNode.GetLocation(),
            Identifier = typeNode.Identifier.ToString(),
            TimeToGenerate = stopwatch.Elapsed,
        };

        return result;
    }

    private static string FormatXmlComments(string commentXml)
    {
        if (string.IsNullOrWhiteSpace(commentXml))
        {
            return string.Empty;
        }

        // Split the XML comment into lines
        var lines = commentXml.Split(["\r\n", "\r", "\n"], StringSplitOptions.RemoveEmptyEntries)
            .Select(l => $"        /// {l.Trim()}")
            .ToArray();

        // Join the lines back together
        var result = string.Join($"{NewLine}", lines.Skip(1).Take(lines.Length - 2));

        return result;
    }
}
