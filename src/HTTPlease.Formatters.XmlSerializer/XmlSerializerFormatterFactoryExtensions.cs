using System;

namespace HTTPlease
{
	/// <summary>
	///		XML serialiser request extension methods for <see cref="HttpRequestFactory"/>.
	/// </summary>
	public static class XmlSerializerFormatterFactoryExtensions
    {
		/// <summary>
		///		Create a new HTTP request that expects and uses XML as its primary format.
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
		public static HttpRequest XmlSerializer(this HttpRequestFactory requestFactory, string requestUri)
		{
			if (requestFactory == null)
				throw new ArgumentNullException(nameof(requestFactory));

			if (String.IsNullOrWhiteSpace(requestUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'requestUri'.", nameof(requestUri));

			return
				requestFactory.Create(requestUri)
					.ExpectXml()
					.UseXmlSerializer();
		}
		
		/// <summary>
		///		Create a new HTTP request that expects and uses XML as its primary format.
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
		public static HttpRequest XmlSerializer(this HttpRequestFactory requestFactory, Uri requestUri)
		{
			if (requestFactory == null)
				throw new ArgumentNullException(nameof(requestFactory));

			if (requestUri == null)
				throw new ArgumentNullException(nameof(requestUri));

			return
				requestFactory.Create(requestUri)
					.ExpectXml()
					.UseXmlSerializer();
		}
	}
}
