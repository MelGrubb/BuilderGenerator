using BuilderGenerator;
using System;

namespace BuilderGenerator.UnitTests
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        internal string InternalString { get; set; }

        [Obsolete]
        public string MiddleName { get; set; }
    }

    [BuilderFor(typeof(Person), false, false)]
    public partial class PersonBuilderWithoutInternals
    {
    }
}
