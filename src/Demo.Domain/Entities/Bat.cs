using BuilderGenerator.Common.Attributes;

namespace Demo.Domain.Entities
{
    [GenerateBuilder]
    public class Bat
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}