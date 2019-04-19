﻿using System;

namespace HTTPlease
{
    using Formatters;
    using Formatters.Xml;
    
    /// <summary>
    ///    Extension methods for content formatters.
    /// </summary>
    public static class XmlFormatterExtensions
    {
        /// <summary>
        ///    Add the XML content formatter.
        /// </summary>
        /// <param name="formatters">
        ///    The content formatter collection.
        /// </param>
        /// <returns>
        ///    The content formatter collection (enables method-chaining).
        /// </returns>
        public static IFormatterCollection AddXmlFormatter(this IFormatterCollection formatters)
        {
            if (formatters == null)
                throw new ArgumentNullException(nameof(formatters));

            formatters.Add(new XmlFormatter());

            return formatters;
        }
    }
}
