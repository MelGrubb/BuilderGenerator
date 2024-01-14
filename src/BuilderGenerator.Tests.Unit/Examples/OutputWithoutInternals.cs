#nullable disable

//-------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by BuilderGenerator at 2024-01-14T14:03:02 in 4.9846ms
// </auto-generated>
//-------------------------------------------------------------------------------------
using System.CodeDom.Compiler;
using BuilderGenerator;

namespace BuilderGenerator.UnitTests
{
    public partial class PersonBuilderWithoutInternals : BuilderGenerator.Builder<BuilderGenerator.UnitTests.Person>
    {
        public System.Lazy<string> FirstName = new System.Lazy<string>(() => default(string));
        public System.Lazy<string> LastName = new System.Lazy<string>(() => default(string));

        /// <summary>Initializes a new instance of the <see cref="PersonBuilderWithoutInternals"/> class using the provided <see cref="BuilderGenerator.UnitTests.Person" /> for the Object value.</summary>
        /// <param name="value">The <see cref="BuilderGenerator.UnitTests.Person" /> instance to build on.</param>
        /// <remarks>Note: <paramref name="value" /> is not simply a template. The actual instance will be remembered and returned.</remarks>
        public PersonBuilderWithoutInternals(BuilderGenerator.UnitTests.Person value = null)
        {
            if (value != null)
            {
                WithObject(value);
            }
        }

        /// <summary>Populates this instance with values from the provided example.</summary>
        /// <remarks>This is a shallow clone, and does not traverse the example object creating builders for its properties.</remarks>
        public PersonBuilderWithoutInternals WithValuesFrom(BuilderGenerator.UnitTests.Person example)
        {
            WithFirstName(example.FirstName);
            WithLastName(example.LastName);

            return this;
        }

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

        /// <summary>Sets the object to be returned by this instance.</summary>
        /// <param name="value">The object to be returned.</param>
        /// <returns>A reference to this builder instance.</returns>
        public PersonBuilderWithoutInternals WithObject(BuilderGenerator.UnitTests.Person value)
        {
            Object = new System.Lazy<BuilderGenerator.UnitTests.Person>(() => value);
            WithValuesFrom(value);

            return this;
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
