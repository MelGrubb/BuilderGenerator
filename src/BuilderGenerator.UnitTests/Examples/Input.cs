using BuilderGenerator;

namespace BuilderGenerator.UnitTests
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [BuilderFor(typeof(Person))]
    public partial class PersonBuilder
    {
    }
}
