using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BuilderGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace BuilderGenerator
{
    [Generator]
    internal class BuilderGenerator : ISourceGenerator
    {
        private static Dictionary<string, string>? _templates;

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(
                x =>
                {
                    // Inject the base Builder class and GenerateBuilder attribute
                    x.AddSource("Builder", SourceText.From(Templates.Builder, Encoding.UTF8));
                    x.AddSource("Attribute", SourceText.From(Templates.GenerateBuilderAttribute, Encoding.UTF8));
                });

            context.RegisterForSyntaxNotifications(() => new BuilderGeneratorSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not BuilderGeneratorSyntaxReceiver receiver)
            {
                return;
            }

            var templates = GetTemplates(context);
            var templateParser = new TemplateParser();

            foreach (var @class in receiver.Classes.Where(x => x != null))
            {
                var targetClassProperties = @class.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                    .Where(x => x.Modifiers.All(m => m.ToString() != "static") && x.HasSetter())
                    .ToArray();

                var targetClassName = @class.Identifier.Text;
                var targetClassFullName = @class.FullName();
                var generatedCodeAttributeTemplate = templates["GeneratedCodeAttributeTemplate"];
                var builderClassUsingBlock = ((CompilationUnitSyntax)@class.SyntaxTree.GetRoot()).Usings.ToString();
                var builderClassNamespace = @class.Namespace() + ".Builders";
                var builderClassName = $"{targetClassName}Builder";

                templateParser.SetTag("UsingBlock", builderClassUsingBlock);
                templateParser.SetTag("Namespace", builderClassNamespace);
                templateParser.SetTag("GeneratedCodeAttributeTemplate", generatedCodeAttributeTemplate);
                templateParser.SetTag("BuilderName", builderClassName);
                templateParser.SetTag("ClassName", targetClassName);
                templateParser.SetTag("ClassFullName", targetClassFullName);

                var builderClassProperties = BuildProperties(templateParser, templates, targetClassProperties);
                var builderClassBuildMethod = BuildBuildMethod(templateParser, templates, targetClassProperties);
                var builderClassWithMethods = BuildWithMethods(templateParser, templates, targetClassProperties);

                templateParser.SetTag("Properties", builderClassProperties);
                templateParser.SetTag("BuildMethod", builderClassBuildMethod);
                templateParser.SetTag("WithMethods", builderClassWithMethods);

                var source = templateParser.ParseString(Templates.BuilderClassTemplate);
                context.AddSource($"{targetClassName}Builder.generated.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private Dictionary<string, string> GetTemplates(GeneratorExecutionContext context)
        {
            if (_templates == null)
            {
                var fields = typeof(Templates).GetFields(BindingFlags.Public | BindingFlags.Static);
                _templates = fields.ToDictionary(x => x.Name, x => (string)x.GetValue(null));

                var overrides = context.AdditionalFiles.ToDictionary(x => Path.GetFileNameWithoutExtension(x.Path), x => File.ReadAllText(x.Path));

                foreach (var template in overrides)
                {
                    _templates[template.Key] = template.Value;
                }
            }

            return _templates;
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
