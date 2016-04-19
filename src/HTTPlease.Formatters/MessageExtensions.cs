using System;
using System.Net.Http;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		Formatter-related extension methods for <see cref="HttpRequestMessage"/> / <see cref="HttpResponseMessage"/>.
	/// </summary>
	public static class MessageExtensions
    {
		/// <summary>
		///		Get the message's <see cref="IFormatterCollection"/> (if any).
		/// </summary>
		/// <param name="message">
		///		The HTTP request message.
		/// </param>
		/// <returns>
		///		The media-type formatters, or <c>null</c> if the message does not have any associated formatters.
		/// </returns>
		public static IFormatterCollection GetMediaTypeFormatters(this HttpRequestMessage message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			object mediaTypeFormatters;
			message.Properties.TryGetValue(MessageProperties.MediaTypeFormatters, out mediaTypeFormatters);

			return (IFormatterCollection)mediaTypeFormatters;
		}

		/// <summary>
		///		Set the message's <see cref="IFormatterCollection"/>.
		/// </summary>
		/// <param name="message">
		///		The HTTP request message.
		/// </param>
		/// <param name="mediaTypeFormatters">
		///		The media-type formatters (if any).
		/// </param>
		public static void SetMediaTypeFormatters(this HttpRequestMessage message, IFormatterCollection mediaTypeFormatters)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			message.Properties[MessageProperties.MediaTypeFormatters] = mediaTypeFormatters;
		}
	}
}
