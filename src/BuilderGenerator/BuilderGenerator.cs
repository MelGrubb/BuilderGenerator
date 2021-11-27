using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
        //private static INamedTypeSymbol? _builderForAttributeSymbol;
        //private static INamedTypeSymbol? _builderSymbol;
        //private static INamedTypeSymbol? GetBuilderForAttributeSymbol(Compilation compilation) => _builderForAttributeSymbol ??= compilation.GetTypeByMetadataName(BuilderForAttributeFullName);
        //private static INamedTypeSymbol? GetBuilderSymbol(Compilation compilation) => _builderSymbol ??= compilation.GetTypeByMetadataName(Templates.BuilderBaseClass);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classDeclarations = context.SyntaxProvider.CreateSyntaxProvider(Predicate, Transform).Where(static node => node is not null);
            var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());
            context.RegisterSourceOutput(compilationAndClasses, static (spc, source) => Execute(source.Item1, source.Item2!, spc));

            context.RegisterPostInitializationOutput(
                x =>
                {
                    // Inject base classes that never change
                    x.AddSource(nameof(Templates.BuilderBaseClass), SourceText.From(Templates.BuilderBaseClass, Encoding.UTF8));
                    x.AddSource(nameof(Templates.BuilderForAttribute), SourceText.From(Templates.BuilderForAttribute, Encoding.UTF8));
                });
        }

        private static void Execute(Compilation compilation, ImmutableArray<TypeDeclarationSyntax> classes, SourceProductionContext context)
        {
            if (classes.IsDefaultOrEmpty) { return; }

            var distinctClasses = classes.Distinct();

            foreach (var typeDeclaration in distinctClasses)
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
                templateParser.SetTag("BuilderName", builderClassName);
                templateParser.SetTag("ClassName", targetClassName);
                templateParser.SetTag("ClassFullName", targetClassFullName);

                var builderClassProperties = GenerateProperties(templateParser, targetClassProperties);
                var builderClassBuildMethod = GenerateBuildMethod(templateParser, targetClassProperties);
                var builderClassWithMethods = GenerateWithMethods(templateParser, targetClassProperties);

                templateParser.SetTag("Properties", builderClassProperties);
                templateParser.SetTag("BuildMethod", builderClassBuildMethod);
                templateParser.SetTag("WithMethods", builderClassWithMethods);

                var source = templateParser.ParseString(Templates.BuilderClass);

                context.AddSource($"{builderClassName}.generated.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private static string GenerateBuildMethod(TemplateParser templateParser, IEnumerable<IPropertySymbol> properties)
        {
            var setters = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Name);

                        return templateParser.ParseString(Templates.BuildMethodSetter);
                    }));

            templateParser.SetTag("Setters", setters);
            var result = templateParser.ParseString(Templates.BuildMethod);

            return result;
        }

        private static string GenerateProperties(TemplateParser templateParser, IEnumerable<IPropertySymbol> properties)
        {
            var result = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Name);
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(Templates.Property);
                    }));

            return result;
        }

        private static string GenerateWithMethods(TemplateParser templateParser, IEnumerable<IPropertySymbol> properties)
        {
            var result = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Name.ToString());
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(Templates.WithMethod);
                    }));

            return result;
        }

        private static bool Predicate(SyntaxNode node, CancellationToken _) => node is TypeDeclarationSyntax { AttributeLists.Count: > 0 };

        private static TypeDeclarationSyntax? Transform(GeneratorSyntaxContext context, CancellationToken token)
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
    }
}
