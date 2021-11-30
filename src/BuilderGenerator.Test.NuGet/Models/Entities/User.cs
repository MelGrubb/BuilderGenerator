using System.Collections.Generic;
#nullable enable

namespace BuilderGenerator.Test.NuGet.Models.Entities
{
    public class User : Entity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
