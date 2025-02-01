//-------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by BuilderGenerator at {{GenerationTime:u}} in {{GenerationDuration}}ms
// </auto-generated>
//-------------------------------------------------------------------------------------
using System.CodeDom.Compiler;
{{BuilderClassUsingBlock}}

#nullable disable
#pragma warning disable 618 // Suppress complaints about obsolete properties.

namespace {{BuilderClassNamespace}}
{
    {{BuilderClassAccessibility}} partial class {{BuilderClassName}} : BuilderGenerator.Builder<{{TargetClassFullName}}>
    {
        /// <summary>Gets or sets the object returned by this builder.</summary>
        /// <value>The constructed object.</value>
        protected System.Lazy<{{TargetClassFullName}}> {{TargetClassName}} { get; set; }

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
{{WithValuesFromMethod}}
{{WithMethods}}
    }
}
