namespace BuilderGenerator
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class BuilderForAttribute : System.Attribute
    {
        public bool IncludeInternals { get; }
        public bool IncludeObsolete { get; }
        public System.Type Type { get; }

        public BuilderForAttribute(System.Type type, bool includeInternals = false, bool includeObsolete = false)
        {
            IncludeInternals = includeInternals;
            IncludeObsolete = includeObsolete;
            Type = type;
        }
    }
}
