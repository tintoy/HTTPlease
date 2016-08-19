using Newtonsoft.Json;
using System;

namespace HTTPlease.Formatters
{
	using Json;

	/// <summary>
	///		Formatter-related extension methods for <see cref="HttpRequest"/> / <see cref="HttpRequest{TContext}"/>.
	/// </summary>
	public static class RequestExtensions
	{
		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, configuring it to only use the JSON formatters.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <param name="serializerSettings">
		///		<see cref="JsonSerializerSettings"/> used to configure the formatter's behaviour.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest UseJson(this HttpRequest request, JsonSerializerSettings serializerSettings = null)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			// TODO: Clear all existing formatters, first.

			return request.WithFormatter(new JsonFormatter
			{
				SerializerSettings = serializerSettings
			});
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest{TContext}"/>, configuring it to only use the JSON formatters.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The <see cref="HttpRequest{TContext}"/>.
		/// </param>
		/// <param name="serializerSettings">
		///		<see cref="JsonSerializerSettings"/> used to configure the formatter's behaviour.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest<TContext> UseJson<TContext>(this HttpRequest<TContext> request, JsonSerializerSettings serializerSettings = null)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			// TODO: Clear all existing formatters, first.

			return request.WithFormatter(new JsonFormatter
			{
				SerializerSettings = serializerSettings
			});
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, configuring it to accept the JSON ("application/json") media type.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest ExpectJson(this HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return request.AcceptMediaType(WellKnownMediaTypes.Json);
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, configuring it to accept the JSON ("application/json") media type.
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
		public static HttpRequest<TContext> ExpectJson<TContext>(this HttpRequest<TContext> request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return request.AcceptMediaType(WellKnownMediaTypes.Json);
		}
	}
}
