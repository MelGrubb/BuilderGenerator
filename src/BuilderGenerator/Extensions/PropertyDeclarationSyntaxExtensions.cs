using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BuilderGenerator.Extensions
{
    public static class PropertyDeclarationSyntaxExtensions
    {
        /// <summary>
        ///     Determines whether this <see cref="PropertyDeclarationSyntax" /> has a setter.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        ///     <c>true</c> if the specified property has setter; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasSetter(this PropertyDeclarationSyntax property) => property.AccessorList?.Accessors.Any(x => x.Kind() == SyntaxKind.SetAccessorDeclaration) == true;

        public static bool ImplementsInterface<T>(this PropertyDeclarationSyntax property) =>
            // TODO: Fill in the blanks.
            true;
    }
}
