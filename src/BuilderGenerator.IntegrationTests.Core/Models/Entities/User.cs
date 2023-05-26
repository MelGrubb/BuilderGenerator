#nullable enable

using System.Collections.Generic;

namespace BuilderGenerator.IntegrationTests.Core.Models.Entities;

public partial class User : AuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
}

public partial class User
{
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
