using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new BuilderGeneratorSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (BuilderGeneratorSyntaxReceiver) context.SyntaxReceiver;
            var types = receiver?.Classes ?? Enumerable.Empty<TypeDeclarationSyntax>();

            foreach (var type in types)
            {
                if (type is null) return;

                var usingBlock = ((CompilationUnitSyntax) type.SyntaxTree.GetRoot()).Usings.ToString();

                // TODO: Get the namespace of the original class and add "Builders" to it.
                var @namespace = "Demo.Domain.Entities.Builders";
                var className = type.Identifier.Text;
                var classFullName = type.FullName();
                var builderName = $"{className}Builder";

                var propertyInfos = type.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                    .Where(x => x.Modifiers.All(m => m.ToString() != "static"));

                var source = Templates.BuilderClassTemplate
                    .Replace("{UsingBlock}", usingBlock)
                    .Replace("{Namespace}", @namespace)
                    .Replace("{GeneratedAttribute}", Templates.GeneratedAttribute)
                    .Replace("{BuilderName}", builderName)
                    .Replace("{ClassName}", className)
                    .Replace("{ClassFullName}", classFullName)
                    .Replace("{Properties}", BuildProperties(propertyInfos))
                    .Replace("{BuildMethod}", BuildBuildMethod(type, propertyInfos))
                    .Replace("{WithMethods}", BuildWithMethods(propertyInfos, builderName, classFullName))
                    ;

                context.AddSource($"{builderName}.generated.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private string BuildWithMethods(IEnumerable<PropertyDeclarationSyntax> propertyInfos, string builderName, string classFullName)
        {
            var withPostProcessAction = Templates.WithPostProcessActionTemplate
                .Replace("{GeneratedAttribute}", Templates.GeneratedAttribute)
                .Replace("{BuilderName}", builderName)
                .Replace("{ClassFullName}", classFullName);

            var withMethods = string.Join(Environment.NewLine, propertyInfos.Select(x => Templates.WithMethodTemplate
                .Replace("{GeneratedAttribute}", Templates.GeneratedAttribute)
                .Replace("{BuilderName}", builderName)
                .Replace("{ClassFullName}", classFullName)
                .Replace("{PropertyName}", x.Identifier.ToString())
                .Replace("{PropertyType}", x.Type.ToString())
            ));

            return withPostProcessAction + Environment.NewLine + withMethods;
        }

        private string BuildBuildMethod(TypeDeclarationSyntax type,
            IEnumerable<PropertyDeclarationSyntax> propertyInfos)
        {
            var fullName = type.FullName();

            var setters = string.Join(Environment.NewLine,
                propertyInfos.Select(x => $"                        {x.Identifier} = {x.Identifier}.Value,"));

            return Templates.BuildMethodTemplate
                .Replace("{GeneratedAttribute}", Templates.GeneratedAttribute)
                .Replace("{ClassFullName}", fullName)
                .Replace("{Setters}", setters);
        }

        private static string BuildProperties(IEnumerable<PropertyDeclarationSyntax> propertyInfos)
        {
            return string.Join(Environment.NewLine, propertyInfos.Select(x => Templates.PropertyTemplate
                .Replace("{GeneratedAttribute}", Templates.GeneratedAttribute)
                .Replace("{PropertyName}", x.Identifier.ToString())
                .Replace("{PropertyType}", x.Type.ToString())
            ));
        }
    }
}