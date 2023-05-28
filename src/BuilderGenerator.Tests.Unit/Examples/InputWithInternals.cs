using BuilderGenerator;

namespace BuilderGenerator.UnitTests
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        internal string InternalString { get; set; }
    }

    [BuilderFor(typeof(Person), true)]
    public partial class PersonBuilderWithInternals
    {
    }
}
