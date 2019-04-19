﻿using System;

namespace HTTPlease.Core.Templates
{
    /// <summary>
    ///    The base class for template segments that represent components of a URI's query.
    /// </summary>
    abstract class QuerySegment
        : TemplateSegment
    {
        /// <summary>
        ///    The name of the query parameter that the segment represents.
        /// </summary>
        readonly string _queryParameterName;

        /// <summary>
        ///    Create a new query segment.
        /// </summary>
        /// <param name="queryParameterName">
        ///    The name of the query parameter that the segment represents.
        /// </param>
        protected QuerySegment(string queryParameterName)
        {
            if (String.IsNullOrWhiteSpace(queryParameterName))
                throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'queryParameterName'.", nameof(queryParameterName));

            _queryParameterName = queryParameterName;
        }

        /// <summary>
        ///    The name of the query parameter that the segment represents.
        /// </summary>
        public string Name
        {
            get
            {
                return _queryParameterName;
            }
        }

        /// <summary>
        /// Escape the specified text according to the template segment type's escaping rules.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        /// <returns>The escaped text.</returns>
        protected override string Escape(string text)
        {
            if (text == null)
                return text;

            return Uri.EscapeDataString(text);
        }
    }
}
