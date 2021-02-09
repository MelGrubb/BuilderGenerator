using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BuilderGenerator
{
    internal class BuilderGeneratorSyntaxReceiver : ISyntaxReceiver
    {
        public List<TypeDeclarationSyntax> Classes { get; } = new();

        public ClassDeclarationSyntax ClassToAugment { get; private set; }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not ClassDeclarationSyntax @class || !@class.AttributeLists.Any(x =>
                x.Attributes.Any(a => a.Name.ToString().Equals("GenerateBuilder")))) return;
            
            Classes.Add(@class);
        }
    }
}