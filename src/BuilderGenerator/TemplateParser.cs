using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BuilderGenerator
{
    /// <summary>Simple, lightweight template parser.</summary>
    /// <remarks>
    ///     Replaces ASP-style template tags in the form "{{tag}}" with their values from the TagValues dictionary.
    /// </remarks>
    public class TemplateParser
    {
        private const string TagPattern = @"{{\s*(?<1>.*?)\s*}}";
        public TemplateParser()
        {
            TagValues = new Dictionary<string, Lazy<object>>(StringComparer.CurrentCultureIgnoreCase);
        }

        private Dictionary<string, Lazy<object>> TagValues { get; }

        public void SetTag(string key, object value) => TagValues[key] = new Lazy<object>(() => value);

        public void SetTag(string key, Func<object> func) => TagValues[key] = new Lazy<object>(func);

        /// <summary>Parses a string, replacing all tags with their matching values from the TagValues property.</summary>
        /// <param name="template">A string containing the text to be parsed.</param>
        /// <returns>A string containing the resulting text after parsing <paramref name="template" />.</returns>
        /// <remarks>
        ///     Tags take the ASP-style form of {{Tag}}.  Additionally, formatting
        ///     may be specified in the template in the same syntax as the String.Format method accepts.
        /// </remarks>
        /// <example>
        ///     The following code results in "  1/2/2000" being written to the console.
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
                            CultureInfo.CurrentCulture,
                            "{0" + (tagParts.Length == 3 ? tagParts[1] + tagParts[2] : string.Empty) + "}",
                            tagObject.Value)
                        : string.Empty;
                });
        }
    }
}
