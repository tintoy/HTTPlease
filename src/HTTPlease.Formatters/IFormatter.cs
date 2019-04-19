using System.Collections.Generic;

namespace HTTPlease.Formatters
{
    /// <summary>
    ///    Represents a content formatter.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        ///    Content types supported by the formatter.
        /// </summary>
        ISet<string> SupportedMediaTypes { get; }
    }
}
