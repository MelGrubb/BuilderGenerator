using System;
using System.Collections.Generic;
using BuilderGenerator.Test.Local.Builders;
using BuilderGenerator.Test.Local.Models.Entities;
using NUnit.Framework;
using Shouldly;

namespace BuilderGenerator.Test.Local.Tests
{
    [TestFixture]
    public class BuilderTests
    {
        private string _firstName;
        private string _middleName;
        private Guid _id;
        private string _lastName;
        private User _result;

        [Test]
        public void UserBuilder_can_set_properties()
        {
            _result.Id.ShouldBe(_id);
            _result.FirstName.ShouldBe(_firstName);
            _result.MiddleName.ShouldBe(_middleName);
            _result.LastName.ShouldBe(_lastName);
        }

        [Test]
        public void Simple_returns_a_UserBuilder() => UserBuilder.Simple().ShouldBeOfType<UserBuilder>();

        [Test]
        public void Typical_returns_a_UserBuilder() => UserBuilder.Typical().ShouldBeOfType<UserBuilder>();

        [Test]
        public void Simple_does_not_populate_Orders()
        {
            var actual = UserBuilder.Simple().Build();
            ShouldBeTestExtensions.ShouldBeOfType<User>(actual);
            ShouldBeNullExtensions.ShouldBeNull<ICollection<Order>>(actual.Orders);
        }

        [Test]
        public void Typical_populates_Orders()
        {
            var actual = UserBuilder.Typical().Build();
            ShouldBeTestExtensions.ShouldBeOfType<User>(actual);
            ShouldBeNullExtensions.ShouldNotBeNull<ICollection<Order>>(actual.Orders);
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
}
