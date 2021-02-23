using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BuilderGenerator.Attributes;
using BuilderGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace BuilderGenerator
{
    [Generator]
    internal class BuilderGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif

            context.RegisterForSyntaxNotifications(() => new BuilderGeneratorSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (BuilderGeneratorSyntaxReceiver)context.SyntaxReceiver!;
            var templateParser = new TemplateParser();

            foreach (var @class in receiver.Classes.Where(x => x != null))
            {
                var templates = GetTemplates(context, @class);

                var properties = @class.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                    .Where(x => x.Modifiers.All(m => m.ToString() != "static") && x.HasSetter())
                    .ToArray();

                templateParser.SetTag("UsingBlock", ((CompilationUnitSyntax)@class.SyntaxTree.GetRoot()).Usings.ToString());
                templateParser.SetTag("Namespace", @class.Namespace() + ".Builders");
                templateParser.SetTag("GeneratedAttribute", templates);
                templateParser.SetTag("BuilderName", $"{@class.Identifier.Text}Builder");
                templateParser.SetTag("ClassName", @class.Identifier.Text);
                templateParser.SetTag("ClassFullName", @class.FullName());
                templateParser.SetTag("Properties", BuildProperties(templateParser, templates, properties));
                templateParser.SetTag("BuildMethod", BuildBuildMethod(templateParser, templates, properties));
                templateParser.SetTag("WithMethods", BuildWithMethods(templateParser, templates, properties));

                var source = templateParser.ParseString(Templates.BuilderClassTemplate);
                context.AddSource($"{@class.Identifier.Text}Builder.generated.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private Dictionary<string, string> GetTemplates(GeneratorExecutionContext context, ClassDeclarationSyntax @class)
        {
            var attributeSyntax = @class.AttributeLists.SelectMany(x => x.Attributes).Single(x => x.Name + "Attribute" == nameof(GenerateBuilderAttribute));

            var syntaxResults = new Dictionary<string, string?>
            {
                { "GeneratedAttribute", GetAttributeValue(attributeSyntax, nameof(GenerateBuilderAttribute.GeneratedAttributeTemplate)) },
                { "BuilderClassTemplate", GetAttributeValue(attributeSyntax, nameof(GenerateBuilderAttribute.BuilderClassTemplate)) },
                { "BuildMethodSetterTemplate", GetAttributeValue(attributeSyntax, nameof(GenerateBuilderAttribute.BuildMethodSetterTemplate)) },
                { "BuildMethodTemplate", GetAttributeValue(attributeSyntax, nameof(GenerateBuilderAttribute.BuildMethodTemplate)) },
                //{ "PropertyTemplate", GetAttributeValue(attributeSyntax, nameof(GenerateBuilderAttribute.PropertyTemplate)) },
                //{ "WithMethodTemplate", GetAttributeValue(attributeSyntax, nameof(GenerateBuilderAttribute.WithMethodTemplate)) },
                //{ "WithPostProcessActionTemplate", GetAttributeValue(attributeSyntax, nameof(GenerateBuilderAttribute.WithPostProcessActionTemplate)) },
            };

            var model = context.Compilation.GetSemanticModel(@class.SyntaxTree);
            var classSymbol = model.GetDeclaredSymbol(@class) as INamedTypeSymbol;
            var attributeData = classSymbol?.GetAttributes().FirstOrDefault(x => $"{x.AttributeClass?.Name}Attribute" == nameof(GenerateBuilderAttribute));

            var symbolResults = new Dictionary<string, string?>
            {
                { "GeneratedAttribute", GetAttributeValue(attributeData, nameof(GenerateBuilderAttribute.GeneratedAttributeTemplate)) },
                { "BuilderClassTemplate", GetAttributeValue(attributeData, nameof(GenerateBuilderAttribute.BuilderClassTemplate)) },
                { "BuildMethodSetterTemplate", GetAttributeValue(attributeData, nameof(GenerateBuilderAttribute.BuildMethodSetterTemplate)) },
                { "BuildMethodTemplate", GetAttributeValue(attributeData, nameof(GenerateBuilderAttribute.BuildMethodTemplate)) },
                //{ "PropertyTemplate", GetAttributeValue(attributeData, nameof(GenerateBuilderAttribute.PropertyTemplate)) },
                //{ "WithMethodTemplate", GetAttributeValue(attributeData, nameof(GenerateBuilderAttribute.WithMethodTemplate)) },
                //{ "WithPostProcessActionTemplate", GetAttributeValue(attributeData, nameof(GenerateBuilderAttribute.WithPostProcessActionTemplate)) },
            };

            return syntaxResults;
        }

        private string? GetAttributeValue(AttributeData? attribute, string name)
        {
            var constructorArguments = attribute?.ConstructorArguments.ToList();
            var namedArguments = attribute?.NamedArguments.ToList();

            return null;
        }

        private string? GetAttributeValue(AttributeSyntax attribute, string name)
        {
            var argument = attribute.ArgumentList?.Arguments.SingleOrDefault(x => x.NameColon?.Name.Identifier.ValueText.Equals(name, StringComparison.InvariantCultureIgnoreCase) == true);
            var expression = argument?.Expression as LiteralExpressionSyntax;
            var value = expression?.Token.ValueText;

            return value;
        }

        private string BuildWithMethods(TemplateParser templateParser, Dictionary<string, string> templates, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var withPostProcessAction = templateParser.ParseString(templates["WithPostProcessActionTemplate"]);

            var withMethods = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Identifier.ToString());
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(templates["WithMethodTemplate"]);
                    }));

            var result = withMethods + Environment.NewLine + withPostProcessAction;

            return result;
        }

        private string BuildBuildMethod(TemplateParser templateParser, Dictionary<string, string> templates, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var setters = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Identifier);

                        return templateParser.ParseString(templates["BuildMethodSetterTemplate"]);
                    }));

            templateParser.SetTag("Setters", setters);
            var result = templateParser.ParseString(templates["BuildMethodTemplate"]);

            return result;
        }

        private static string BuildProperties(TemplateParser templateParser, Dictionary<string, string> templates, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var result = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Identifier.ToString());
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(templates["PropertyTemplate"]);
                    }));

            return result;
        }
    }
}
