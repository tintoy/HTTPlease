using System;

namespace HTTPlease.Formatters
{
	using Xml;

	/// <summary>
	///		Formatter-related extension methods for <see cref="HttpRequest"/> / <see cref="HttpRequest{TContext}"/>.
	/// </summary>
	public static class RequestExtensions
	{
		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, configuring it to only use the <see cref="XmlSerializerFormatter"/>.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest UseXmlSerializer(this HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			// TODO: Clear all existing formatters, first.

			return request.WithFormatter(new XmlSerializerFormatter());
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest{TContext}"/>, configuring it to only use the <see cref="XmlSerializerFormatter"/>.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The <see cref="HttpRequest{TContext}"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest<TContext> UseXmlSerializer<TContext>(this HttpRequest<TContext> request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			// TODO: Clear all existing formatters, first.

			return request.WithFormatter(new XmlSerializerFormatter());
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, configuring it to accept the XML ("application/json") media type.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest ExpectXml(this HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return request.AcceptMediaType(WellKnownMediaTypes.Xml);
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, configuring it to accept the XML ("application/json") media type.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest<TContext> ExpectXml<TContext>(this HttpRequest<TContext> request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return request.AcceptMediaType(WellKnownMediaTypes.Xml);
		}
	}
}
