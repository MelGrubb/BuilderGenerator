using System.Collections.Generic;
using BuilderGenerator.Common.Attributes;

namespace Demo.Entities
{
    [GenerateBuilder]
    public class Foo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Abbreviation { get; set; }
        public Bar Bar { get; set; }

        public string[] ReadWriteArray { get; set; } = new string[0];
        public string[] ReadOnlyArray { get; } = new string[0];

        public List<string> ReadWriteList { get; set; } = new();
        public List<string> ReadOnlyList { get; } = new();

        public HashSet<string> ReadWriteHashSet { get; set; } = new();
        public HashSet<string> ReadOnlyHashSet { get; } = new();

        public ICollection<string> ReadWriteCollection { get; set; } = new List<string>();
        public ICollection<string> ReadOnlyCollection { get; } = new List<string>();

        public IEnumerable<string> ReadWriteEnumerable { get; set; } = new List<string>();
        public IEnumerable<string> ReadOnlyEnumerable { get; } = new List<string>();
    }
}
