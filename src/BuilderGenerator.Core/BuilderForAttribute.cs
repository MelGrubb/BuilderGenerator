namespace BuilderGenerator.Core
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class BuilderForAttribute(System.Type type, bool includeInternals = false, bool includeObsolete = false) : System.Attribute
    {
        public bool IncludeInternals { get; } = includeInternals;
        public bool IncludeObsolete { get; } = includeObsolete;
        public System.Type Type { get; } = type;
    }
}
