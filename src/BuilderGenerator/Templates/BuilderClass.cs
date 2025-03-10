//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by BuilderGenerator{{GenerationTime}}{{GenerationDuration}}.
//
//     Changes to this file may cause incorrect behavior
//     and will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.CodeDom.Compiler;
{{BuilderClassUsingBlock}}
#nullable disable
#pragma warning disable 618 // Suppress complaints about obsolete properties.
#pragma warning disable CS8669 // Nullability of type of parameter doesn't match target type.

namespace {{BuilderClassNamespace}}
{
    {{BuilderClassAccessibility}} partial class {{BuilderClassName}} : BuilderGenerator.Builder<{{TargetClassFullName}}>
    {
        /// <summary>Gets or sets the object returned by this builder.</summary>
        /// <value>The constructed object.</value>
        public System.Lazy<{{TargetClassFullName}}> {{TargetClassName}} { get; set; }

        // <summary>Gets or sets the action to be performed when an object is built.</summary>
        // <remarks>
        //     This is only performed when an object is created from scratch for the first time.
        //     When the object value has been injected from outside, this action will not be called.
        // </remarks>
        public System.Action<{{TargetClassFullName}}> PostBuildAction { get; set; }
{{Properties}}

        /// <summary>Initializes a new instance of the <see cref="{{BuilderClassName}}"/> class using the provided <see cref="{{TargetClassFullName}}" /> for the value.</summary>
        /// <param name="value">The <see cref="{{TargetClassFullName}}" /> instance to build on.</param>
        /// <remarks>Note: <paramref name="value" /> is not simply a template. The actual instance will be remembered and returned.</remarks>
        public {{BuilderClassName}}({{TargetClassFullName}} value = null)
        {
            if (value != null)
            {
                With{{TargetClassName}}(value);
            }
        }

{{BuildMethod}}
{{WithObjectMethod}}
        public {{BuilderClassName}} WithPostBuildAction(System.Action<{{TargetClassFullName}}> action)
        {
            PostBuildAction = action;
            return this;
        }

{{WithValuesFromMethod}}
{{WithMethods}}
    }
}
