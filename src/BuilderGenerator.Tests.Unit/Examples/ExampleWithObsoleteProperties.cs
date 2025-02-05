using System;

namespace BuilderGenerator.Tests.Unit.Examples;

/// <summary>An example class with various properties.</summary>
public class Person
{
    /// <summary>
    ///     The Person's first name.
    /// </summary>
    /// <remarks>This was a multi-line, indented summary in the original source code.</remarks>
    public string FirstName { get; set; }

    /// <summary>The Person's last name.</summary>
    /// <remarks>This was a single-line summary in the original source code.</remarks>
    public string LastName { get; set; }

    /// <summary>A string property marked as obsolete to test the "includeObsolete" attribute parameter.</summary>
    /// <remarks>This was a single-line summary in the original source code.</remarks>
    public string MiddleName { get; set; }

    /// <summary>A string with internal accessibility to test the "includeInternals" attribute parameter.</summary>
    /// <remarks>This was a single-line summary in the original source code.</remarks>
    internal string InternalString { get; set; }

    /// <summary>A string with internal accessibility to test the "includeInternals" attribute parameter.</summary>
    /// <remarks>This was a single-line summary in the original source code.</remarks>
    [Obsolete]
    public string ObsoleteString { get; set; }
}

/// <summary>An example builder that includes obsolete properties.</summary>
[BuilderFor(typeof(Person), includeObsolete: true)]
public partial class PersonBuilderWithObsolete
{
}
