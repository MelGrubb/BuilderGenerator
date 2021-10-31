using System;
using BuilderGenerator.Test.NuGet.Framework;
using BuilderGenerator.Test.NuGet.Models.Entities;
using BuilderGenerator.Test.NuGet.Models.Entities.Builders;
using NUnit.Framework;

namespace BuilderGenerator.Test.NuGet.Tests
{
    [TestFixture]
    public class BuilderTests
    {
        private string _firstName;
        private Guid _id;
        private string _lastName;
        private User _result;

        [Test]
        public void UserBuilder_can_set_properties()
        {
            Assert.AreEqual(_id, _result.Id);
            Assert.AreEqual(_firstName, _result.FirstName);
            Assert.AreEqual(_lastName, _result.LastName);
        }

        [Test]
        public void UserBuilder_exists()
        {
            var actual = new UserBuilder();
            Assert.IsInstanceOf<UserBuilder>(actual);
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            _id = Guid.NewGuid();
            _firstName = GetRandom.FirstName();
            _lastName = GetRandom.LastName();

            _result = new UserBuilder()
                .WithId(_id)
                .WithFirstName(_firstName)
                .WithLastName(_lastName)
                .WithMiddleName("Bob")
                .Build();
        }
    }
}
