using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HTTPlease
{
	using ValueProviders;

	/// <summary>
	///     Represents a template for building HTTP requests with lazily-resolved values extracted from a specific context.
	/// </summary>
	/// <typeparam name="TContext">
	///     The type of object used by the request when resolving deferred values.
	/// </typeparam>
	public interface ITypedHttpRequest<TContext>
		: IHttpRequest
	{
		/// <summary>
		///		Actions (if any) to perform on the outgoing request message.
		/// </summary>
		IReadOnlyList<RequestAction<TContext>> RequestActions { get; }

		/// <summary>
		///     The request's URI template parameters (if any).
		/// </summary>
		IReadOnlyDictionary<string, IValueProvider<TContext, string>> TemplateParameters { get; }

		/// <summary>
		///     The request's query parameters (if any).
		/// </summary>
		IReadOnlyDictionary<string, IValueProvider<TContext, string>> QueryParameters { get; }

		/// <summary>
		///     Build and configure a new HTTP request message.
		/// </summary>
		/// <param name="httpMethod">
		///     The HTTP request method to use.
		/// </param>
		/// <param name="context">
		///     The <typeparamref name="TContext" /> to use as the context for resolving any deferred template or query parameters.
		/// </param>
		/// <param name="body">
		///     Optional <see cref="HttpContent" /> representing the request body.
		/// </param>
		/// <param name="baseUri">
		///     An optional base URI to use if the request builder does not already have an absolute request URI.
		/// </param>
		/// <returns>
		///     The configured <see cref="HttpRequestMessage" />.
		/// </returns>
		HttpRequestMessage BuildRequestMessage(HttpMethod httpMethod, TContext context, HttpContent body = null, Uri baseUri = null);
	}
}