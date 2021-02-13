using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BuilderGenerator.Extensions
{
    public static class ClassDeclarationSyntaxExtensions
    {
        public static string FullName(this ClassDeclarationSyntax source)
        {
            var items = new List<string>();
            var parent = source.Parent;

            while (parent.IsKind(SyntaxKind.ClassDeclaration))
            {
                var parentClass = (ClassDeclarationSyntax)parent;
                items.Add(parentClass.Identifier.Text);

                parent = parent.Parent;
            }

            var nameSpace = (NamespaceDeclarationSyntax)parent;
            var sb = new StringBuilder().Append(nameSpace.Name).Append(".");
            items.Reverse();
            items.ForEach(i => { sb.Append(i).Append("+"); });
            sb.Append(source.Identifier.Text);
            var result = sb.ToString();

            return result;
        }

        public static bool HasDefaultConstructor(this ClassDeclarationSyntax @class) => @class.DescendantNodes().OfType<ConstructorDeclarationSyntax>().Any(x => !x.ParameterList.Parameters.Any());
    }
}
