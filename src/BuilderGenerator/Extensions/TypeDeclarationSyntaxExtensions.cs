using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BuilderGenerator.Extensions
{
    public static class TypeDeclarationSyntaxExtensions
    {
        public static string FullName(this TypeDeclarationSyntax source)
        {
            var items = new List<string>();
            var parent = source.Parent;

            while (parent.IsKind(SyntaxKind.ClassDeclaration))
            {
                var parentClass = (TypeDeclarationSyntax) parent;
                items.Add(parentClass.Identifier.Text);

                parent = parent.Parent;
            }

            var nameSpace = (NamespaceDeclarationSyntax) parent;
            var sb = new StringBuilder().Append(nameSpace.Name).Append(".");
            items.Reverse();
            items.ForEach(i => { sb.Append(i).Append("+"); });
            sb.Append(source.Identifier.Text);
            var result = sb.ToString();

            return result;
        }
    }
}