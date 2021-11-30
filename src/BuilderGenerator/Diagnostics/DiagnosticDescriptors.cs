using System;
using Microsoft.CodeAnalysis;

namespace BuilderGenerator.Diagnostics
{
    internal class DiagnosticDescriptors
    {
        internal static Diagnostic TargetClassIsAbstract(Location location, string identifier) => Diagnostic.Create(new DiagnosticDescriptor("BGN002", "Target class cannot be abstract", $"Cannot generate a builder for '{identifier}' because it is abstract", "BuilderGenerator", DiagnosticSeverity.Error, true), location);

        internal static Diagnostic UnexpectedErrorDiagnostic(Exception exception, Location location, string identifier) => Diagnostic.Create(new DiagnosticDescriptor("BGN001", "Unexpected error", $"An error occurred while generating a builder for '{identifier}'\n{exception.Message}", "BuilderGenerator", DiagnosticSeverity.Error, true), location);
    }
}
