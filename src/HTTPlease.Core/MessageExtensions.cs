using System;
using System.Net.Http;

namespace HTTPlease
{
    /// <summary>
    ///     Extension methods for <see cref="HttpRequestMessage"/> / <see cref="HttpResponseMessage"/>.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        ///     Determine whether the request message has been configured for a streamed response.
        /// </summary>
        /// <param name="message">
        ///     The HTTP request message.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if the request message has been configured for a streamed response; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStreamed(this HttpRequestMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            
            object isStreamedValue;
            message.Properties.TryGetValue(MessageProperties.IsStreamed, out isStreamedValue);

            return (isStreamedValue as bool?) ?? false;
        }

        /// <summary>
        ///     Mark the request message as configured for a streamed / buffered response.
        /// </summary>
        /// <param name="message">
        ///     The HTTP request message.
        /// </param>
        /// <param name="isStreamed">
        ///     If <c>true</c>, the request message is configured for a streamed response; otherwise, it is configured for a buffered response.
        /// </param>
        /// <returns>
        ///     The HTTP request message (enables inline use).
        /// </returns>
        public static HttpRequestMessage MarkAsStreamed(this HttpRequestMessage message, bool isStreamed = true)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            message.Properties[MessageProperties.IsStreamed] = isStreamed;

            return message;
        }
    }
}