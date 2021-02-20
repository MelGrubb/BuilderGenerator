using BuilderGenerator.Attributes;

namespace Demo.Entities
{
    [GenerateBuilder]
    public class Bat
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
