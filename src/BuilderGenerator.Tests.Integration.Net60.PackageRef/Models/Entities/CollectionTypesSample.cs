using System;
using System.Collections.Generic;

namespace BuilderGenerator.Tests.Integration.Net60.PackageRef.Models.Entities;

public class CollectionTypesSample
{
    public string[] ReadOnlyArray { get; } = [];
    public ICollection<string> ReadOnlyCollection { get; } = new List<string>();
    public IEnumerable<string> ReadOnlyEnumerable { get; } = new List<string>();
    public HashSet<string> ReadOnlyHashSet { get; } = [];
    public List<string> ReadOnlyList { get; } = [];
    public string[] ReadWriteArray { get; set; } = [];
    public ICollection<string> ReadWriteCollection { get; set; } = new List<string>();
    public IEnumerable<string> ReadWriteEnumerable { get; set; } = new List<string>();
    public HashSet<string> ReadWriteHashSet { get; set; } = [];
    public List<string> ReadWriteList { get; set; } = [];
}
