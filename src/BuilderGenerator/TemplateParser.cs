using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BuilderGenerator;

/// <summary>Simple, lightweight template parser.</summary>
/// <remarks>
///     Replaces ASP-style template tags in the form "{{tag}}" with their values from the TagValues dictionary.
///     This accomplishes the same thing as a series of chained calls to .Replace, but only makes a single pass over the source template.
/// </remarks>
internal class TemplateParser
{
    private const string TagPattern = @"{{\s*(?<1>.*?)\s*}}";

    /// <summary>Initializes a new instance of the <see cref="TemplateParser" /> class.</summary>
    public TemplateParser()
    {
        TagValues = new Dictionary<string, Lazy<object>>(StringComparer.CurrentCultureIgnoreCase);
    }

    /// <summary>Contains the tag/value pairs.</summary>
    /// <value>The tag values to be inserted into the template.</value>
    private Dictionary<string, Lazy<object>> TagValues { get; }

    /// <summary>Parses a string, replacing all tags with their matching values from the TagValues property.</summary>
    /// <param name="template">A string containing the text to be parsed.</param>
    /// <returns>A string containing the resulting text after parsing <paramref name="template" />.</returns>
    /// <remarks>
    ///     Tags take the ASP-style form of {{Tag}}.  Additionally, formatting
    ///     may be specified in the template in the same syntax as the String.Format method accepts.
    /// </remarks>
    /// <example>
    ///     The following code results in "1/2/2000" being written to the console.
    ///     <code>
    ///         var parser = new TemplateParser();
    ///         parser.TagValues["Date"] = new DateTime(2000, 1, 2);
    ///         Console.WriteLine(parser.ParseString("{{Date,10:d}}");
    ///     </code>
    /// </example>
    public string ParseString(string template)
    {
        _ = template ?? throw new ArgumentNullException(nameof(template));

        return Regex.Replace(
            template,
            TagPattern,
            match =>
            {
                var tag = match.Groups[1].Value;
                var tagParts = new Regex("([,:])").Split(tag, 2);
                var tagName = tagParts[0];

                Lazy<object> tagObject;

                return TagValues.TryGetValue(tagName, out tagObject)
                    ? string.Format(
                        CultureInfo.InvariantCulture,
                        "{0" + (tagParts.Length == 3 ? tagParts[1] + tagParts[2] : string.Empty) + "}",
                        tagObject.Value)
                    : string.Empty;
            });
    }

    /// <summary>Defines a tag, and the value that should replace it.</summary>
    /// <param name="tag">The tag name.</param>
    /// <param name="value">The tag value.</param>
    public void SetTag(string tag, object value)
    {
        TagValues[tag] = new Lazy<object>(() => value);
    }

    /// <summary>Defines a tag, and the value that should replace it.</summary>
    /// <param name="tag">The tag name.</param>
    /// <param name="func">A <see cref="Func{T}" /> that returns the tag value.</param>
    public void SetTag(string tag, Func<object> func)
    {
        TagValues[tag] = new Lazy<object>(func);
    }
}
