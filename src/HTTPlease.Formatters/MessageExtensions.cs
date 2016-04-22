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
		///		The content formatters, or <c>null</c> if the message does not have any associated formatters.
		/// </returns>
		public static IFormatterCollection GetFormatters(this HttpRequestMessage message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			object contentFormatters;
			message.Properties.TryGetValue(MessageProperties.ContentFormatters, out contentFormatters);

			return (IFormatterCollection)contentFormatters;
		}

		/// <summary>
		///		Get the message's <see cref="IFormatterCollection"/> (if any).
		/// </summary>
		/// <param name="message">
		///		The HTTP request message.
		/// </param>
		/// <returns>
		///		The content formatters, or <c>null</c> if the message does not have any associated formatters.
		/// </returns>
		/// <remarks>
		///		Can only be called on an <see cref="HttpResponseMessage"/> whose <see cref="HttpResponseMessage.RequestMessage"/> contains a valid <see cref="HttpRequestMessage"/>.
		/// </remarks>
		public static IFormatterCollection GetFormatters(this HttpResponseMessage message)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			HttpRequestMessage requestMessage = message.RequestMessage;
			if (requestMessage == null)
				throw new InvalidOperationException("This operation is only valid on a response message produced by invoking an HttpRequest (the response message does not have an associated request message).");

			object contentFormatters;
			message.RequestMessage.Properties.TryGetValue(MessageProperties.ContentFormatters, out contentFormatters);

			return (IFormatterCollection)contentFormatters;
		}

		/// <summary>
		///		Set the message's <see cref="IFormatterCollection"/>.
		/// </summary>
		/// <param name="message">
		///		The HTTP request message.
		/// </param>
		/// <param name="contentFormatters">
		///		The content formatters (if any).
		/// </param>
		public static void SetFormatters(this HttpRequestMessage message, IFormatterCollection contentFormatters)
		{
			if (message == null)
				throw new ArgumentNullException(nameof(message));

			message.Properties[MessageProperties.ContentFormatters] = contentFormatters;
		}
	}
}
