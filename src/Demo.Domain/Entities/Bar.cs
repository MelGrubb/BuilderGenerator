using BuilderGenerator.Common.Attributes;

namespace Demo.Domain.Entities
{
    [GenerateBuilder]
    public class Bar
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}