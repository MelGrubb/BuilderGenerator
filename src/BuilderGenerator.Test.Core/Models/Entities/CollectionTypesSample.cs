using System;
using System.Collections.Generic;

namespace BuilderGenerator.Test.Core.Models.Entities
{
    public class CollectionTypesSample
    {
        public string[] ReadOnlyArray { get; } = Array.Empty<string>();
        public ICollection<string> ReadOnlyCollection { get; } = new List<string>();
        public IEnumerable<string> ReadOnlyEnumerable { get; } = new List<string>();
        public HashSet<string> ReadOnlyHashSet { get; } = new HashSet<string>();
        public List<string> ReadOnlyList { get; } = new List<string>();

        public string[] ReadWriteArray { get; set; } = Array.Empty<string>();

        public ICollection<string> ReadWriteCollection { get; set; } = new List<string>();

        public IEnumerable<string> ReadWriteEnumerable { get; set; } = new List<string>();

        public HashSet<string> ReadWriteHashSet { get; set; } = new HashSet<string>();

        public List<string> ReadWriteList { get; set; } = new List<string>();
    }
}
