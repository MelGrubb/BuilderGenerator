using System.Collections.Generic;
using System.Linq;
using BuilderGenerator.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BuilderGenerator
{
    internal class BuilderGeneratorSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> Classes { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax @class && @class.AttributeLists.Any(x => x.Attributes.Any(a => a.Name + "Attribute" == nameof(GenerateBuilderAttribute))))
            {
                Classes.Add(@class);
            }
        }
    }
}
