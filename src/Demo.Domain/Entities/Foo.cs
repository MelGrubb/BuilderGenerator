using BuilderGenerator.Common.Attributes;

namespace Demo.Domain.Entities
{
    [GenerateBuilder]
    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}