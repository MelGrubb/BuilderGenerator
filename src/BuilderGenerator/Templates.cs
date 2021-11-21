using System.Reflection;

// ReSharper disable UnusedMember.Global

namespace BuilderGenerator
{
    internal static class Templates
    {
        internal const string BuilderClassTemplate = @"using System;
using System.CodeDom.Compiler;
{{UsingBlock}}
#nullable enable

namespace {{Namespace}}
{
    {{GeneratedCodeAttributeTemplate}}
    public partial class {{BuilderName}} : BuilderGenerator.Builder<{{ClassFullName}}>
    {
        public Action<{{ClassFullName}}>? PostProcessAction  { get; set; }
{{Properties}}
{{BuildMethod}}
{{WithMethods}}
    }
}";

        internal const string BuildMethodSetterTemplate = "                        {{PropertyName}} = {{PropertyName}}.Value,";

        internal const string BuildMethodTemplate = @"
        {{GeneratedCodeAttributeTemplate}}
        public override {{ClassFullName}} Build()
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new Lazy<{{ClassFullName}}>(() => 
                {
                    var result = new {{ClassFullName}} 
                    {
{{Setters}}
                    };

                    return result;
                });

                if (PostProcessAction != null)
                {
                    PostProcessAction(Object.Value);
                }
            }

            return Object.Value;
        }";

        internal const string PropertyTemplate = @"        public Lazy<{{PropertyType}}> {{PropertyName}} = new Lazy<{{PropertyType}}>(() => default({{PropertyType}}));";

        internal const string WithMethodTemplate = @"
        {{GeneratedCodeAttributeTemplate}}
        public {{BuilderName}} With{{PropertyName}}({{PropertyType}} value)
        {
            return With{{PropertyName}}(() => value);
        }

        {{GeneratedCodeAttributeTemplate}}
        public {{BuilderName}} With{{PropertyName}}(Func<{{PropertyType}}> func)
        {
            {{PropertyName}} = new Lazy<{{PropertyType}}>(func);
            return this;
        }

        {{GeneratedCodeAttributeTemplate}}
        public {{BuilderName}} Without{{PropertyName}}()
        {                    
            {{PropertyName}} = new Lazy<{{PropertyType}}>(() => default({{PropertyType}}));
            return this;
        }";

        internal const string WithPostProcessActionTemplate = @"
        {{GeneratedCodeAttributeTemplate}}
        public {{BuilderName}} WithPostProcessAction(Action<{{ClassFullName}}> action)
        {
            PostProcessAction = action;

            return this;
        }

        {{GeneratedCodeAttributeTemplate}}
        public T WithPostProcessAction<T>(Action<{{ClassFullName}}> action) where T : {{BuilderName}}
        {
            PostProcessAction = action;

            return (T)this;
        }";

        internal const string BuilderForAttributeTemplate = @"namespace BuilderGenerator
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class BuilderForAttribute : System.Attribute
    {
        public System.Type Type { get; }

        public BuilderForAttribute(System.Type type)
        {
            this.Type = type;
        }
    }
}";

        internal const string BuilderTemplate = @"#nullable enable

namespace BuilderGenerator
{
    /// <summary>Base class for object builder classes.</summary>
    /// <typeparam name=""T"">The type of the objects built by this builder.</typeparam>
    public abstract class Builder<T> where T : class
    {
        /// <summary>Gets or sets the object returned by this builder.</summary>
        /// <value>The constructed object.</value>
        #pragma warning disable CA1720 // Identifier contains type name
        protected System.Lazy<T>? Object { get; set; }
        #pragma warning restore CA1720 // Identifier contains type name

        /// <summary>Builds the object instance.</summary>
        /// <returns>The constructed object.</returns>
        public abstract T Build();

        /// <summary>Sets the object to be returned by this instance.</summary>
        /// <param name=""value"">The object to be returned.</param>
        /// <returns>A reference to this builder instance.</returns>
        public Builder<T> WithObject(T value)
        {
            Object = new System.Lazy<T>(value);

            return this;
        }
    }
}";

        internal const string GenerateBuilderAttributeTemplate = @"namespace BuilderGenerator
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class GenerateBuilderAttribute : System.Attribute
    {
    }
}";

        internal static readonly string GeneratedCodeAttributeTemplate = $"[GeneratedCode(\"BuilderGenerator\", \"{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}\")]";
    }
}
