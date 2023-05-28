using System.CodeDom.Compiler;
using BuilderGenerator;

namespace BuilderGenerator.UnitTests
{
    public partial class PersonBuilderWithInternals : BuilderGenerator.Builder<BuilderGenerator.UnitTests.Person>
    {
        public System.Lazy<string> FirstName = new System.Lazy<string>(() => default(string));
        public System.Lazy<string> InternalString = new System.Lazy<string>(() => default(string));
        public System.Lazy<string> LastName = new System.Lazy<string>(() => default(string));

        public override BuilderGenerator.UnitTests.Person Build()
        {
            if (Object?.IsValueCreated != true)
            {
                Object = new System.Lazy<BuilderGenerator.UnitTests.Person>(new BuilderGenerator.UnitTests.Person());
            }

            Object.Value.FirstName = FirstName.Value;
            Object.Value.InternalString = InternalString.Value;
            Object.Value.LastName = LastName.Value;

            PostProcess(Object.Value);

            return Object.Value;
        }

        /// <summary>Sets the object to be returned by this instance.</summary>
        /// <param name="value">The object to be returned.</param>
        /// <returns>A reference to this builder instance.</returns>
        public override Builder<BuilderGenerator.UnitTests.Person> WithObject(BuilderGenerator.UnitTests.Person value)
        {
            base.WithObject(value);

            WithFirstName(value.FirstName);
            WithInternalString(value.InternalString);
            WithLastName(value.LastName);

            return this;
        }

        public PersonBuilderWithInternals WithFirstName(string value)
        {
            return WithFirstName(() => value);
        }

        public PersonBuilderWithInternals WithFirstName(System.Func<string> func)
        {
            FirstName = new System.Lazy<string>(func);
            return this;
        }

        public PersonBuilderWithInternals WithoutFirstName()
        {
            FirstName = new System.Lazy<string>(() => default(string));
            return this;
        }

        public PersonBuilderWithInternals WithInternalString(string value)
        {
            return WithInternalString(() => value);
        }

        public PersonBuilderWithInternals WithInternalString(System.Func<string> func)
        {
            InternalString = new System.Lazy<string>(func);
            return this;
        }

        public PersonBuilderWithInternals WithoutInternalString()
        {
            InternalString = new System.Lazy<string>(() => default(string));
            return this;
        }

        public PersonBuilderWithInternals WithLastName(string value)
        {
            return WithLastName(() => value);
        }

        public PersonBuilderWithInternals WithLastName(System.Func<string> func)
        {
            LastName = new System.Lazy<string>(func);
            return this;
        }

        public PersonBuilderWithInternals WithoutLastName()
        {
            LastName = new System.Lazy<string>(() => default(string));
            return this;
        }
    }
}
