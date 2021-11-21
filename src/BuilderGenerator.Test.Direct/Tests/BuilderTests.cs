using System;
using BuilderGenerator.Test.Direct.Models.Entities;
using BuilderGenerator.Test.Direct.Models.Entities.Builders;
using NUnit.Framework;

namespace BuilderGenerator.Test.Direct.Tests
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
        public void UserBuilder_exists()
        {
            var actual = new UserBuilder();
            Assert.IsInstanceOf<UserBuilder>(actual);
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            _id = Guid.NewGuid();
            _firstName = Guid.NewGuid().ToString();
            _middleName = Guid.NewGuid().ToString();
            _lastName = Guid.NewGuid().ToString();

            _result = new UserBuilder()
                .WithId(_id)
                .WithFirstName(_firstName)
                .WithMiddleName(_middleName)
                .WithLastName(_lastName)
                .Build();
        }
    }
}
