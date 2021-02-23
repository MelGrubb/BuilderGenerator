using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BuilderGenerator.Extensions
{
    internal static class PropertyDeclarationSyntaxExtensions
    {
        /// <summary>
        ///     Determines whether this <see cref="PropertyDeclarationSyntax" /> has a setter.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        ///     <c>true</c> if the specified property has setter; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasSetter(this PropertyDeclarationSyntax property) => property.AccessorList?.Accessors.Any(x => x.Kind() == SyntaxKind.SetAccessorDeclaration) == true;
    }
}
