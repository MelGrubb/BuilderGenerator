using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var receiver = (BuilderGeneratorSyntaxReceiver)context.SyntaxReceiver;
            var types = receiver?.Classes ?? Enumerable.Empty<TypeDeclarationSyntax>();
            var templateParser = new TemplateParser();

            foreach (var type in types)
            {
                if (type is null)
                {
                    return;
                }

                var propertyInfos = type.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                    .Where(x => x.Modifiers.All(m => m.ToString() != "static"))
                    .ToArray();

                templateParser.SetTag("UsingBlock", ((CompilationUnitSyntax)type.SyntaxTree.GetRoot()).Usings.ToString());
                templateParser.SetTag("Namespace", type.Namespace() + ".Builders");
                templateParser.SetTag("GeneratedAttribute", Templates.GeneratedAttribute);
                templateParser.SetTag("BuilderName", $"{type.Identifier.Text}Builder");
                templateParser.SetTag("ClassName", type.Identifier.Text);
                templateParser.SetTag("ClassFullName", type.FullName());
                templateParser.SetTag("Properties", BuildProperties(templateParser, propertyInfos));
                templateParser.SetTag("BuildMethod", BuildBuildMethod(templateParser, propertyInfos));
                templateParser.SetTag("WithMethods", BuildWithMethods(templateParser, propertyInfos));

                var source = templateParser.ParseString(Templates.BuilderClassTemplate);
                context.AddSource($"{type.Identifier.Text}Builder.generated.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private string BuildWithMethods(TemplateParser templateParser, IEnumerable<PropertyDeclarationSyntax> propertyInfos)
        {
            var withPostProcessAction = templateParser.ParseString(Templates.WithPostProcessActionTemplate);

            var withMethods = string.Join(
                Environment.NewLine,
                propertyInfos.Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Identifier.ToString());
                        templateParser.SetTag("PropertyType", x.Type.ToString());

                        return templateParser.ParseString(Templates.WithMethodTemplate);
                    }));

            var result = withMethods + Environment.NewLine + withPostProcessAction;

            return result;
        }

        private string BuildBuildMethod(TemplateParser templateParser, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var setters = string.Join(
                Environment.NewLine,
                properties.Where(x => x.HasSetter()).Select(
                    x =>
                    {
                        templateParser.SetTag("PropertyName", x.Identifier);

                        return templateParser.ParseString(Templates.BuildMethodSetterTemplate);
                    }));

            templateParser.SetTag("Setters", setters);
            var result = templateParser.ParseString(Templates.BuildMethodTemplate);

            return result;
        }

        private static string BuildProperties(TemplateParser templateParser, IEnumerable<PropertyDeclarationSyntax> properties)
        {
            var result = string.Join(
                Environment.NewLine,
                properties
                    .Where(x => x.HasSetter())
                    .Select(
                        x =>
                        {
                            templateParser.SetTag("PropertyName", x.Identifier.ToString());
                            templateParser.SetTag("PropertyType", x.Type.ToString());

                            return templateParser.ParseString(Templates.PropertyTemplate);
                        }));

            return result;
        }
    }
}
