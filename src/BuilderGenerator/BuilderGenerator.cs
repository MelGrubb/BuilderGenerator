using System;
using System.Collections.Generic;
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
    public class BuilderGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) => context.RegisterForSyntaxNotifications(() => new BuilderGeneratorSyntaxReceiver());

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (BuilderGeneratorSyntaxReceiver)context.SyntaxReceiver!;
            var templateParser = new TemplateParser();

            foreach (var @class in receiver.Classes.Where(x => x != null))
            {
                // TODO: Extract the templates from the attribute
                var attribute = @class.AttributeLists.SelectMany(x => x.Attributes).Single(x => x.Name + "Attribute" == nameof(GenerateBuilderAttribute));
                var templates = new Templates();

                var properties = @class.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                    .Where(x => x.Modifiers.All(m => m.ToString() != "static") && x.HasSetter())
                    .ToArray();

                templateParser.SetTag("UsingBlock", ((CompilationUnitSyntax)@class.SyntaxTree.GetRoot()).Usings.ToString());
                templateParser.SetTag("Namespace", @class.Namespace() + ".Builders");
                templateParser.SetTag("GeneratedAttribute", templates.GeneratedAttribute);
                templateParser.SetTag("BuilderName", $"{@class.Identifier.Text}Builder");
                templateParser.SetTag("ClassName", @class.Identifier.Text);
                templateParser.SetTag("ClassFullName", @class.FullName());
                templateParser.SetTag("Properties", BuildProperties(templateParser, templates, properties));
                templateParser.SetTag("BuildMethod", BuildBuildMethod(templateParser, templates, properties));
                templateParser.SetTag("WithMethods", BuildWithMethods(templateParser, templates, properties));

                var source = templateParser.ParseString(templates.BuilderClassTemplate);
                context.AddSource($"{@class.Identifier.Text}Builder.generated.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private string BuildWithMethods(TemplateParser templateParser, Templates templates, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var withPostProcessAction = templateParser.ParseString(templates.WithPostProcessActionTemplate);

            var withMethods = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Identifier.ToString());
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(templates.WithMethodTemplate);
                    }));

            var result = withMethods + Environment.NewLine + withPostProcessAction;

            return result;
        }

        private string BuildBuildMethod(TemplateParser templateParser, Templates templates, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var setters = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Identifier);

                        return templateParser.ParseString(templates.BuildMethodSetterTemplate);
                    }));

            templateParser.SetTag("Setters", setters);
            var result = templateParser.ParseString(templates.BuildMethodTemplate);

            return result;
        }

        private static string BuildProperties(TemplateParser templateParser, Templates templates, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var result = string.Join(
                Environment.NewLine,
                properties.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Identifier.ToString());
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(templates.PropertyTemplate);
                    }));

            return result;
        }
    }
}
