using BuilderGenerator.Attributes;

namespace Demo.Entities
{
    [GenerateBuilder]
    public class Baz
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
