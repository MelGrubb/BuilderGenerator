using System;
using System.Collections.Generic;
#nullable enable

namespace BuilderGenerator.Sample.Models.Entities
{
    [GenerateBuilder]
    public class User
    {
        public string? FirstName { get; set; }
        public Guid Id { get; set; }

        public string? LastName { get; set; }

        //public string? MiddleName { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
