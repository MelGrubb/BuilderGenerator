using System;
using BuilderGenerator.Tests.Integration.Net70.ProjectRef.Builders;
using BuilderGenerator.Tests.Integration.Net70.ProjectRef.Models.Entities;
using NUnit.Framework;
using Shouldly;

namespace BuilderGenerator.Tests.Integration.Net70.ProjectRef.Tests;

[TestFixture]
public class UserBuilderTests
{
    private string _firstName = null!;
    private Guid _id;
    private string _lastName = null!;
    private string _middleName = null!;
    private User _result = null!;

    [Test]
    public void Simple_does_not_populate_Orders()
    {
        var actual = UserBuilder.Simple().Build();
        actual.ShouldBeOfType<User>();
        actual.Orders.ShouldBeNull();
    }

    [Test]
    public void Simple_returns_a_UserBuilder()
    {
        UserBuilder.Simple().ShouldBeOfType<UserBuilder>();
    }

    [Test]
    public void Typical_populates_Orders()
    {
        var actual = UserBuilder.Typical().Build();
        actual.ShouldBeOfType<User>();
        actual.Orders.ShouldNotBeNull();
    }

    [Test]
    public void Typical_returns_a_UserBuilder()
    {
        UserBuilder.Typical().ShouldBeOfType<UserBuilder>();
    }

    [Test]
    public void UserBuilder_can_set_properties()
    {
        _result.Id.ShouldBe(_id);
        _result.FirstName.ShouldBe(_firstName);
        _result.MiddleName.ShouldBe(_middleName);
        _result.LastName.ShouldBe(_lastName);
    }

    [OneTimeSetUp]
    public void SetUp()
    {
        _id = Guid.NewGuid();
        _firstName = Guid.NewGuid().ToString();
        _middleName = Guid.NewGuid().ToString();
        _lastName = Guid.NewGuid().ToString();

        _result = UserBuilder
            .Typical()
            .WithId(_id)
            .WithFirstName(_firstName)
            .WithMiddleName(_middleName)
            .WithLastName(_lastName)
            .Build();
    }
}
