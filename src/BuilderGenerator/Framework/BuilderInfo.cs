using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace BuilderGenerator;

internal record struct BuilderInfo
{
    public Accessibility BuilderClassAccessibility { get; set; }
    public string BuilderClassName { get; set; }
    public string BuilderClassNamespace { get; set; }
    public string BuilderClassUsingBlock { get; set; }
    public string Identifier { get; set; }
    public Location Location { get; set; }
    public List<PropertyInfo> Properties { get; set; }
    public string TargetClassFullName { get; set; }
    public string TargetClassName { get; set; }

    /// <summary>Gets or sets the time it took to generate a particular builder.</summary>
    /// <value>The time to generate the builder class.</value>
    /// <remarks>Note that this property is not included in the hash or Equals methods.</remarks>
    public TimeSpan TimeToGenerate { get; set; }

    public bool Equals(BuilderInfo other)
    {
        var result =
            BuilderClassAccessibility == other.BuilderClassAccessibility
            && BuilderClassName == other.BuilderClassName
            && BuilderClassNamespace == other.BuilderClassNamespace
            && BuilderClassUsingBlock == other.BuilderClassUsingBlock
            && Identifier == other.Identifier
            //&& Location == other.Location
            && TargetClassFullName == other.TargetClassFullName
            && TargetClassName == other.TargetClassName
            && Properties.SequenceEqual(other.Properties);

        return result;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = (hash * 23) + BuilderClassAccessibility.GetHashCode();
            hash = (hash * 23) + BuilderClassName.GetHashCode();
            hash = (hash * 23) + BuilderClassNamespace.GetHashCode();
            hash = (hash * 23) + BuilderClassUsingBlock.GetHashCode();
            hash = (hash * 23) + Identifier.GetHashCode();
            //hash = (hash * 23) + Location.GetHashCode();
            hash = (hash * 23) + TargetClassFullName.GetHashCode();
            hash = (hash * 23) + TargetClassName.GetHashCode();
            hash = (hash * 23) + Properties.Aggregate(hash, (current, property) => (current * 23) + property.GetHashCode());

            return hash;
        }
    }

    internal record struct PropertyInfo
    {
        /// <summary>Gets or sets the accessibility of the target class property.</summary>
        /// <value>The accessibility of the target class property.</value>
        /// <remarks>Although this isn't used directly by the templates themselves, a change in accessibility could result in a re-generation of the builder, so we need this to be part of the hash code.</remarks>
        public Accessibility Accessibility { get; set; }

        /// <summary>Gets or sets the name of the target class property.</summary>
        /// <value>The name of the target class property.</value>
        public string Name { get; set; }

        public string Type { get; set; }
    }
}
