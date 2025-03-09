using System;
using Microsoft.AspNetCore.Identity;

namespace BuilderGenerator.Tests.Integration.Net90.ProjectRef.Models.Entities;

public class CustomIdentityUser : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
}
