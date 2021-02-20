using System;

namespace BuilderGenerator.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateBuilderAttribute : Attribute
    {
        // TODO: Allow passing custom templates via attribute constructor
        //public GenerateBuilderAttribute(Templates? templates = null)
        //{
        //    Templates = templates ?? new Templates();
        //}

        //public Templates Templates { get; }
    }
}
