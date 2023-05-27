#nullable disable

using System.CodeDom.Compiler;
using BuilderGenerator;

namespace BuilderGenerator.UnitTests
{
    public partial class PersonBuilderWithoutInternals : BuilderGenerator.Builder<BuilderGenerator.UnitTests.Person>
    {
        public System.Lazy<string> FirstName = new System.Lazy<string>(() => default(string));
        public System.Lazy<string> LastName = new System.Lazy<string>(() => default(string));

        public override BuilderGenerator.UnitTests.Person Build()
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new System.Lazy<BuilderGenerator.UnitTests.Person>(() =>
                {
                    var result = new BuilderGenerator.UnitTests.Person
                    {
                        FirstName = FirstName.Value,
                        LastName = LastName.Value,
                    };

                    return result;
                });

                PostProcess(Object.Value);
            }

            return Object.Value;
        }

        public PersonBuilderWithoutInternals WithFirstName(string value)
        {
            return WithFirstName(() => value);
        }

        public PersonBuilderWithoutInternals WithFirstName(System.Func<string> func)
        {
            FirstName = new System.Lazy<string>(func);
            return this;
        }

        public PersonBuilderWithoutInternals WithoutFirstName()
        {
            FirstName = new System.Lazy<string>(() => default(string));
            return this;
        }

        public PersonBuilderWithoutInternals WithLastName(string value)
        {
            return WithLastName(() => value);
        }

        public PersonBuilderWithoutInternals WithLastName(System.Func<string> func)
        {
            LastName = new System.Lazy<string>(func);
            return this;
        }

        public PersonBuilderWithoutInternals WithoutLastName()
        {
            LastName = new System.Lazy<string>(() => default(string));
            return this;
        }
    }
}
