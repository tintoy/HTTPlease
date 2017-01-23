using System;

namespace HTTPlease
{
	/// <summary>
	///		A facility for creating <see cref="HttpRequest2"/>s.
	/// </summary>
	public sealed class HttpRequestFactory2
    {
	    /// <summary>
		///		Create a new <see cref="HttpRequestFactory2"/>.
		/// </summary>
		/// <param name="baseRequest">
		///		The <see cref="HttpRequest2"/> used as a base for requests created by the factory.
		/// </param>
	    public HttpRequestFactory2(HttpRequest2 baseRequest)
	    {
		    if (baseRequest == null)
			    throw new ArgumentNullException(nameof(baseRequest));

		    BaseRequest = baseRequest;
	    }

		/// <summary>
		///		The <see cref="HttpRequest2"/> used as a base for requests created by the factory.
		/// </summary>
		public HttpRequest2 BaseRequest { get; }

		/// <summary>
		///		Create a new <see cref="HttpRequest2"/> with the specified request URI.
		/// </summary>
		/// <param name="requestUri">
		///		The request URI.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest2"/>.
		/// </returns>
		public HttpRequest2 Create(Uri requestUri)
		{
			if (requestUri == null)
				throw new ArgumentNullException(nameof(requestUri));

			return BaseRequest.WithUri(requestUri);
		}
    }

	// /// <summary>
	// ///		A facility for creating <see cref="HttpRequest2{TContext}"/>s.
	// /// </summary>
	// /// <typeparam name="TContext">
	// ///		The type of object used as a context for resolving deferred parameters.
	// /// </typeparam>
	// public sealed class HttpRequestFactory2<TContext>
	// {
	// 	/// <summary>
	// 	///		Create a new <see cref="HttpRequestFactory2"/>.
	// 	/// </summary>
	// 	/// <param name="baseRequest">
	// 	///		The <see cref="HttpRequest2{TContext}"/> used as a base for requests created by the factory.
	// 	/// </param>
	// 	public HttpRequest2Factory(HttpRequest2<TContext> baseRequest)
	// 	{
	// 		if (baseRequest == null)
	// 			throw new ArgumentNullException(nameof(baseRequest));

	// 		BaseRequest = baseRequest;
	// 	}

	// 	/// <summary>
	// 	///		The <see cref="HttpRequest2{TContext}"/> used as a base for requests created by the factory.
	// 	/// </summary>
	// 	public HttpRequest2<TContext> BaseRequest { get; }

	// 	/// <summary>
	// 	///		Create a new <see cref="HttpRequest2{TContext}"/> with the specified request URI.
	// 	/// </summary>
	// 	/// <param name="requestUri">
	// 	///		The request URI.
	// 	/// </param>
	// 	/// <returns>
	// 	///		The new <see cref="HttpRequest2{TContext}"/>.
	// 	/// </returns>
	// 	public HttpRequest2<TContext> Create(Uri requestUri)
	// 	{
	// 		if (requestUri == null)
	// 			throw new ArgumentNullException(nameof(requestUri));

	// 		return BaseRequest.WithUri(requestUri);
	// 	}
	// }
}
