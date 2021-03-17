using System;

namespace BuilderGenerator.Demo
{
    [GenerateBuilder]
    public class Foo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid Id { get; set; }
    }
}
