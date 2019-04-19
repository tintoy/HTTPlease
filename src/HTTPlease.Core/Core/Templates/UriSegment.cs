using System;

namespace HTTPlease.Core.Templates
{
    /// <summary>
    ///    The base class for URI template segments that represent segments of the URI.
    /// </summary>
    abstract class UriSegment
        : TemplateSegment
    {
        /// <summary>
        ///    Does the segment represent a directory (i.e. have a trailing slash?).
        /// </summary>
        readonly bool _isDirectory;

        /// <summary>
        ///    Create a new URI segment.
        /// </summary>
        /// <param name="isDirectory">
        ///    Does the segment represent a directory (i.e. have a trailing slash?).
        /// </param>
        protected UriSegment(bool isDirectory)
        {
            _isDirectory = isDirectory;
        }

        /// <summary>
        ///    Does the segment represent a directory (i.e. have a trailing slash?).
        /// </summary>
        public bool IsDirectory
        {
            get
            {
                return _isDirectory;
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

            return Uri.EscapeUriString(text);
        }
    }
}
