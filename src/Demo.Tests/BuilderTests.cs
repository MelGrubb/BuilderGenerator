using Demo.Domain.Entities.Builders;
using NUnit.Framework;

namespace Demo.Tests
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void FooBuilder_exists()
        {
            var actual = new FooBuilder();
            Assert.IsInstanceOf<FooBuilder>(actual);
        }

        [Test]
        public void FooBuilder_can_set_properties()
        {
            var actual = new FooBuilder()
                .WithId(1)
                .WithName("FooBuilderTest")
                .WithAbbreviation("FBT")
                .Build();

            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual("FooBuilderTest", actual.Name);
            Assert.AreEqual("FBT", actual.Abbreviation);
        }

        [Test]
        public void BarBuilder_exists()
        {
            var actual = new BarBuilder();
            Assert.IsInstanceOf<BarBuilder>(actual);
        }

        [Test]
        public void BazBuilder_exists()
        {
            var actual = new BazBuilder();
            Assert.IsInstanceOf<BazBuilder>(actual);
        }

        [Test]
        public void BatBuilder_exists()
        {
            var actual = new BatBuilder();
            Assert.IsInstanceOf<BatBuilder>(actual);
        }
    }
}