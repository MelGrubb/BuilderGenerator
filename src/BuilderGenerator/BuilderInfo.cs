#nullable enable

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace BuilderGenerator;

internal record struct BuilderInfo
{
    // TODO: Implement IComparable and GetHashCode to care only about the combined properties, since List seems to be interpreted using reference equality

    public string BuilderClassAccessibility { get; set; }
    public string BuilderClassName { get; set; }
    public string BuilderClassNamespace { get; set; }
    public string BuilderClassUsingBlock { get; set; }
    public string Identifier { get; set; }

    public Location Location { get; set; }

    public List<PropertyInfo> Properties { get; set; }
    public string TargetClassFullName { get; set; }
    public string TargetClassName { get; set; }

    public bool Equals(BuilderInfo other)
    {
        var result =
            BuilderClassAccessibility == other.BuilderClassAccessibility
            && BuilderClassName == other.BuilderClassName
            && BuilderClassNamespace == other.BuilderClassNamespace
            && BuilderClassUsingBlock == other.BuilderClassUsingBlock
            && Identifier == other.Identifier
            && Location == other.Location
            && TargetClassFullName == other.TargetClassFullName
            && TargetClassName == other.TargetClassName

            // TODO: See if we can live without the OrderBy. Should moving properties around alter the order of the generated builder anyway?
            && Properties.OrderBy(x => x.Name).SequenceEqual(other.Properties.OrderBy(x => x.Name));

        return result;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 27;
            hash = (13 * hash) + BuilderClassAccessibility.GetHashCode();
            hash = (13 * hash) + BuilderClassName.GetHashCode();
            hash = (13 * hash) + BuilderClassNamespace.GetHashCode();
            hash = (13 * hash) + BuilderClassUsingBlock.GetHashCode();
            hash = (13 * hash) + Identifier.GetHashCode();
            hash = (13 * hash) + Location.GetHashCode();
            hash = (13 * hash) + TargetClassFullName.GetHashCode();
            hash = (13 * hash) + TargetClassName.GetHashCode();
            hash = (13 * hash) + Properties.Aggregate(hash, (current, property) => (13 * current) + property.GetHashCode());

            return hash;
        }
    }
}
