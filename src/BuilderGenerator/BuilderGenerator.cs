using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace BuilderGenerator
{
    [Generator]
    internal class BuilderGenerator : IIncrementalGenerator
    {
        private static INamedTypeSymbol? _builderForAttributeSymbol;
        private static INamedTypeSymbol? _builderSymbol;
        private static Dictionary<string, string>? _templates;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            if (_templates == null)
            {
                var fields = typeof(Templates).GetFields(BindingFlags.NonPublic | BindingFlags.Static);
                _templates = fields.ToDictionary(x => x.Name, x => (string)x.GetValue(null));

                // TODO: Figure out the new way to get additional files. Or maybe just ditch the custom template idea altogether.
                //var overrides = context.AdditionalFiles.ToDictionary(x => Path.GetFileNameWithoutExtension(x.Path), x => File.ReadAllText(x.Path));
                //foreach (var template in overrides)
                //{
                //    _templates[template.Key] = template.Value;
                //}
            }

            context.RegisterPostInitializationOutput(
                x =>
                {
                    // Inject base classes that never change
                    x.AddSource("Builder", SourceText.From(_templates["BuilderTemplate"], Encoding.UTF8));
                    x.AddSource("BuilderForAttribute", SourceText.From(_templates["BuilderForAttributeTemplate"], Encoding.UTF8));
                });

            var provider = context.SyntaxProvider
                .CreateSyntaxProvider(IsSyntaxTargetForGeneration, GetSemanticTargetForGeneration)
                .Where(static node => node is not null);

            var nodes = context.CompilationProvider.Combine(provider.Collect());
            context.RegisterSourceOutput(nodes, static (spc, source) => Execute(source.Item1, source.Item2!, spc));
        }

        private static void Execute(Compilation compilation, ImmutableArray<TypeDeclarationSyntax> nodes, SourceProductionContext context)
        {
            if (nodes.IsDefaultOrEmpty) { return; }

            var distinctTypes = nodes.Distinct().Where(x => x != null);

            foreach (var typeDeclaration in distinctTypes)
            {
                var semanticModel = compilation.GetSemanticModel(typeDeclaration.SyntaxTree);
                var typeSymbol = semanticModel.GetDeclaredSymbol(typeDeclaration, context.CancellationToken);
                var templateParser = new TemplateParser();

                if (typeSymbol is not INamedTypeSymbol namedTypeSymbol) { continue; }

                var attributeSymbol = namedTypeSymbol.GetAttributes().SingleOrDefault(x => x.AttributeClass!.Name == "BuilderForAttribute");

                if (attributeSymbol is null) { continue; }

                var targetClassType = attributeSymbol.ConstructorArguments[0];
                var targetClassName = ((ISymbol)targetClassType.Value!).Name;
                var targetClassFullName = targetClassType.Value!.ToString();

                var targetClassProperties = ((INamedTypeSymbol)targetClassType.Value).GetMembers().OfType<IPropertySymbol>()
                    .Where(_ => _.SetMethod is not null && _.SetMethod.DeclaredAccessibility == Accessibility.Public)
                    .ToList();

                var builderClassName = typeSymbol.Name;
                var builderClassNamespace = typeSymbol.ContainingNamespace.ToString();
                var builderClassUsingBlock = ((CompilationUnitSyntax)typeDeclaration.SyntaxTree.GetRoot()).Usings.ToString();

                templateParser.SetTag("UsingBlock", builderClassUsingBlock);
                templateParser.SetTag("Namespace", builderClassNamespace);
                templateParser.SetTag("GeneratedCodeAttributeTemplate", _templates!["GeneratedCodeAttributeTemplate"]);
                templateParser.SetTag("BuilderName", builderClassName);
                templateParser.SetTag("ClassName", targetClassName);
                templateParser.SetTag("ClassFullName", targetClassFullName);

                var builderClassProperties = GenerateProperties(templateParser, _templates, targetClassProperties);
                var builderClassBuildMethod = GenerateBuildMethod(templateParser, _templates, targetClassProperties);
                var builderClassWithMethods = GenerateWithMethods(templateParser, _templates, targetClassProperties);

                templateParser.SetTag("Properties", builderClassProperties);
                templateParser.SetTag("BuildMethod", builderClassBuildMethod);
                templateParser.SetTag("WithMethods", builderClassWithMethods);

                var source = templateParser.ParseString(Templates.BuilderClassTemplate);
                context.AddSource($"{builderClassName}.generated.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private const string BuilderForAttributeFullName = "BuilderGenerator.BuilderForAttribute";

        private static string GenerateBuildMethod(TemplateParser templateParser, Dictionary<string, string> templates, IEnumerable<IPropertySymbol> properties)
        {
            var setters = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Name);

                        return templateParser.ParseString(templates["BuildMethodSetterTemplate"]);
                    }));

            templateParser.SetTag("Setters", setters);
            var result = templateParser.ParseString(templates["BuildMethodTemplate"]);

            return result;
        }

        private static string GenerateProperties(TemplateParser templateParser, Dictionary<string, string> templates, IEnumerable<IPropertySymbol> properties)
        {
            var result = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Name);
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(templates["PropertyTemplate"]);
                    }));

            return result;
        }

        private static string GenerateWithMethods(TemplateParser templateParser, Dictionary<string, string> templates, IEnumerable<IPropertySymbol> properties)
        {
            var withPostProcessAction = templateParser.ParseString(templates["WithPostProcessActionTemplate"]);

            var withMethods = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Name.ToString());
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(templates["WithMethodTemplate"]);
                    }));

            var result = withMethods + Environment.NewLine + withPostProcessAction;

            return result;
        }

        private static INamedTypeSymbol? GetBuilderForAttributeSymbol(Compilation compilation) => _builderForAttributeSymbol ??= compilation.GetTypeByMetadataName(BuilderForAttributeFullName);

        private static INamedTypeSymbol? GetBuilderSymbol(Compilation compilation) => _builderSymbol ??= compilation.GetTypeByMetadataName(_templates!["BuilderTemplate"]);

        private static TypeDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken token)
        {
            var node = context.Node;

            if (node is not TypeDeclarationSyntax typeNode) { return null; }

            var model = context.SemanticModel;

            var typeSymbol = model.GetDeclaredSymbol(typeNode, token);

            if (typeSymbol is not INamedTypeSymbol namedTypeSymbol) { return null; }

            if (namedTypeSymbol.GetAttributes().Any(x => x.AttributeClass?.Name == "BuilderForAttribute"))
            {
                return typeNode;
            }

            return null;
        }

        private static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken _) => node is TypeDeclarationSyntax { AttributeLists.Count: > 0 };
    }
}
