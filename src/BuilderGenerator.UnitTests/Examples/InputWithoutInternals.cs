using BuilderGenerator;

namespace BuilderGenerator.UnitTests
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        internal string InternalString { get; set; }
    }

    [BuilderFor(typeof(Person), false)]
    public partial class PersonBuilderWithoutInternals
    {
    }
}
