using System;
using System.Linq;
using BuilderGenerator.Test.NuGet.Models.Entities;
using BuilderGenerator.Test.NuGet.Models.Entities.Builders;
using NUnit.Framework;

namespace BuilderGenerator.Test.NuGet.Tests
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
            Assert.AreEqual(_id, _result.Id);
            Assert.AreEqual(_firstName, _result.FirstName);
            Assert.AreEqual(_middleName, _result.MiddleName);
            Assert.AreEqual(_lastName, _result.LastName);
        }

        [Test]
        public void Simple_returns_a_UserBuilder()
        {
            var actual = UserBuilder.Simple();
            Assert.IsInstanceOf<UserBuilder>(actual);
        }

        [Test]
        public void Typical_returns_a_UserBuilder()
        {
            var actual = UserBuilder.Typical();
            Assert.IsInstanceOf<UserBuilder>(actual);
        }

        [Test]
        public void Simple_does_not_populate_Orders()
        {
            var actual = UserBuilder.Simple().Build();
            Assert.IsInstanceOf<User>(actual);
            Assert.IsFalse(actual.Orders.Any() == false);
        }

        [Test]
        public void Typical_populates_Orders()
        {
            var actual = UserBuilder.Typical().Build();
            Assert.IsInstanceOf<User>(actual);
            Assert.IsTrue(actual.Orders.Any());
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
