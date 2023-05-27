using Microsoft.CodeAnalysis;

namespace BuilderGenerator;

internal record struct PropertyInfo
{
    /// <summary>Gets or sets the accessibility of the target class property.</summary>
    /// <value>The accessibility of the target class property.</value>
    /// <remarks>Although this isn't used by the templates themselves, a change in accessibility could result in a re-generation of the builder, so we need this to be part of the hash code.</remarks>
    public Accessibility Accessibility { get; set; }

    /// <summary>Gets or sets the name of the target class property.</summary>
    /// <value>The name of the target class property.</value>
    public string Name { get; set; }

    public string Type { get; set; }
}
