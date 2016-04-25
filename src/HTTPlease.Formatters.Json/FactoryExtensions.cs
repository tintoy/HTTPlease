using Newtonsoft.Json;
using System;

namespace HTTPlease.Formatters.Json
{
	/// <summary>
	///		JSON request extension methods for <see cref="HttpRequestFactory"/>.
	/// </summary>
	public static class FactoryExtensions
    {
		/// <summary>
		///		Create a new HTTP request that expects and uses JSON as its primary format.
		/// </summary>
		/// <param name="requestFactory">
		///		The HTTP request factory.
		/// </param>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest JsonFromUri(this HttpRequestFactory requestFactory, string requestUri)
		{
			if (requestFactory == null)
				throw new ArgumentNullException(nameof(requestFactory));

			return requestFactory.JsonFromUri(requestUri, null);
		}

		/// <summary>
		///		Create a new HTTP request that expects and uses JSON as its primary format.
		/// </summary>
		/// <param name="requestFactory">
		///		The HTTP request factory.
		/// </param>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <param name="serializerSettings">
		///		The JSON serialiser settings used by the <see cref="JsonFormatter"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest JsonFromUri(this HttpRequestFactory requestFactory, string requestUri, JsonSerializerSettings serializerSettings)
		{
			if (requestFactory == null)
				throw new ArgumentNullException(nameof(requestFactory));

			if (String.IsNullOrWhiteSpace(requestUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'requestUri'.", nameof(requestUri));

			return
				requestFactory.FromUri(requestUri)
					.ExpectJson()
					.UseJson(serializerSettings);
		}

		/// <summary>
		///		Create a new HTTP request that expects and uses JSON as its primary format.
		/// </summary>
		/// <param name="requestFactory">
		///		The HTTP request factory.
		/// </param>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest JsonFromUri(this HttpRequestFactory requestFactory, Uri requestUri)
		{
			if (requestFactory == null)
				throw new ArgumentNullException(nameof(requestFactory));

			return requestFactory.JsonFromUri(requestUri, null);
		}

		/// <summary>
		///		Create a new HTTP request that expects and uses JSON as its primary format.
		/// </summary>
		/// <param name="requestFactory">
		///		The HTTP request factory.
		/// </param>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <param name="serializerSettings">
		///		The JSON serialiser settings used by the <see cref="JsonFormatter"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest JsonFromUri(this HttpRequestFactory requestFactory, Uri requestUri, JsonSerializerSettings serializerSettings)
		{
			if (requestFactory == null)
				throw new ArgumentNullException(nameof(requestFactory));

			if (requestUri == null)
				throw new ArgumentNullException(nameof(requestUri));

			return
				requestFactory.FromUri(requestUri)
					.ExpectJson()
					.UseJson(serializerSettings);
		}
	}
}
