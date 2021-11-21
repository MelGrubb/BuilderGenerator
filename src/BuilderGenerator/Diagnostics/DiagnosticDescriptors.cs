using Microsoft.CodeAnalysis;

namespace BuilderGenerator.Diagnostics
{
    internal class DiagnosticDescriptors
    {
        public static DiagnosticDescriptor TargetClassIsAbstract { get; } = new("BGN001", "Cannot generate builders for abstract classes", "Cannot generate a builder for '{0}' because it is abstract", "BuilderGenerator", DiagnosticSeverity.Error, true);
    }
}
