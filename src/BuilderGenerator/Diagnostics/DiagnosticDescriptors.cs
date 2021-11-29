using System;
using Microsoft.CodeAnalysis;

namespace BuilderGenerator.Diagnostics
{
    internal class DiagnosticDescriptors
    {
        public static DiagnosticDescriptor TargetClassIsAbstract { get; } = new("BGN001", "Cannot generate builders for abstract classes", "Cannot generate a builder for '{0}' because it is abstract", "BuilderGenerator", DiagnosticSeverity.Error, true);

        internal static Diagnostic UnexpectedErrorDiagnostic(Exception exception, Location location, string identifier) => Diagnostic.Create(new DiagnosticDescriptor("BGN000", "Unexpected error", $"An error occurred while generating a builder for '{identifier}'\n{exception.Message}", "BuilderGenerator", DiagnosticSeverity.Error, true), location);
    }
}
