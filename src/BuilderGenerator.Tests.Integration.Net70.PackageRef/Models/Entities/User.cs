using System.Collections.Generic;

namespace BuilderGenerator.Tests.Integration.Net70.PackageRef.Models.Entities;

public class User : AuditableEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
