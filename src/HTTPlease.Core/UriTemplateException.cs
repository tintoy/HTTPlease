using System;

namespace HTTPlease
{
    /// <summary>
    ///    Exception raised when a <see cref="UriTemplate"/> is invalid or is missing required information.
    /// </summary>
    public class UriTemplateException
        : Exception
    {
        /// <summary>
        ///    Create a new <see cref="UriTemplateException"/>.
        /// </summary>
        /// <param name="messageOrFormat">
        ///    The exception message or message-format specifier.
        /// </param>
        /// <param name="formatArguments">
        ///    Optional message format arguments.
        /// </param>
        public UriTemplateException(string messageOrFormat, params object[] formatArguments)
            : base(String.Format(messageOrFormat, formatArguments))
        {
        }

        /// <summary>
        ///    Create a new <see cref="UriTemplateException"/>.
        /// </summary>
        /// <param name="innerException">
        ///    The exception that caused this exception to be raised.
        /// </param>
        /// <param name="messageOrFormat">
        ///    The exception message or message-format specifier.
        /// </param>
        /// <param name="formatArguments">
        ///    Optional message format arguments.
        /// </param>
        public UriTemplateException(Exception innerException, string messageOrFormat, params object[] formatArguments)
            : base(String.Format(messageOrFormat, formatArguments), innerException)
        {
        }
    }
}
