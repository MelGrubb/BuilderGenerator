using System.Collections.Generic;

namespace BuilderGenerator.Test.NuGet.Models.Entities
{
    [GenerateBuilder]
    public class Sample
    {
        public string[] ReadOnlyArray { get; } = new string[0];
        public ICollection<string> ReadOnlyCollection { get; } = new List<string>();
        public IEnumerable<string> ReadOnlyEnumerable { get; } = new List<string>();
        public HashSet<string> ReadOnlyHashSet { get; } = new();
        public List<string> ReadOnlyList { get; } = new();

        public string[] ReadWriteArray { get; set; } = new string[0];

        public ICollection<string> ReadWriteCollection { get; set; } = new List<string>();

        public IEnumerable<string> ReadWriteEnumerable { get; set; } = new List<string>();

        public HashSet<string> ReadWriteHashSet { get; set; } = new();

        public List<string> ReadWriteList { get; set; } = new();
    }
}
