using System;

namespace HTTPlease
{
    using Formatters;
    using Formatters.Xml;

    /// <summary>
    ///    Extension methods for content formatters.
    /// </summary>
    public static class XmlSerializerFormatterExtensions
    {
        /// <summary>
        ///    Add the XML serialiser content formatter.
        /// </summary>
        /// <param name="formatters">
        ///    The content formatter collection.
        /// </param>
        /// <returns>
        ///    The content formatter collection (enables method-chaining).
        /// </returns>
        public static IFormatterCollection AddXmlSerializerFormatter(this IFormatterCollection formatters)
        {
            if (formatters == null)
                throw new ArgumentNullException(nameof(formatters));

            formatters.Add(new XmlSerializerFormatter());

            return formatters;
        }
    }
}
