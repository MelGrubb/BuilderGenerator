using System;

namespace BuilderGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateBuilderAttribute : Attribute
    {
        public GenerateBuilderAttribute()
        {
        }

        public GenerateBuilderAttribute(string? generatedAttributeTemplate = Templates.GeneratedCodeAttributeTemplate, string? builderClassTemplate = Templates.BuilderClassTemplate, string? buildMethodTemplate = Templates.BuildMethodTemplate, string? buildMethodSetterTemplate = Templates.BuildMethodSetterTemplate)
        {
            BuildMethodTemplate = buildMethodTemplate;
            BuildMethodSetterTemplate = buildMethodSetterTemplate;
            BuilderClassTemplate = builderClassTemplate;
            GeneratedAttributeTemplate = generatedAttributeTemplate;
        }

        public string? BuildMethodTemplate { get; }
        public string? BuildMethodSetterTemplate { get; }
        public string? BuilderClassTemplate { get; }
        public string? GeneratedAttributeTemplate { get; }
    }
}
