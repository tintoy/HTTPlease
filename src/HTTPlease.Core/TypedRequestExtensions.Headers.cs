using System;

namespace HTTPlease
{
	using Core.ValueProviders;

	/// <summary>
	///		<see cref="HttpRequest{TContext}"/> / <see cref="IHttpRequest{TContext}"/> extension methods for HTTP headers.
	/// </summary>
	public static partial class TypedRequestExtensions
    {
		/// <summary>
		///		Create a copy of the request that adds a header to each request.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="headerName">
		///		The header name.
		/// </param>
		/// <param name="headerValue">
		///		The header value.
		/// </param>
		/// <param name="ensureQuoted">
		///		Ensure that the header value is quoted?
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithHeader<TContext>(this HttpRequest<TContext> request, string headerName, string headerValue, bool ensureQuoted = false)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(headerName));

			if (headerValue == null)
				throw new ArgumentNullException(nameof(headerValue));

			return request.WithHeaderFromProvider(headerName,
				ValueProvider<TContext>.FromConstantValue(headerValue),
				ensureQuoted
			);
		}

		/// <summary>
		///		Create a copy of the request that adds a header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <typeparam name="TValue">
		///		The type of header value to add.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="headerName">
		///		The header name.
		/// </param>
		/// <param name="getValue">
		///		A delegate that returns the header value for each request.
		/// </param>
		/// <param name="ensureQuoted">
		///		Ensure that the header value is quoted?
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithHeader<TContext>(this HttpRequest<TContext> request, string headerName, Func<object> getValue, bool ensureQuoted = false)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(headerName));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeaderFromProvider(headerName,
				ValueProvider<TContext>.FromFunction(getValue).Convert().ValueToString(),
				ensureQuoted
			);
		}

		/// <summary>
		///		Create a copy of the request that adds a header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <typeparam name="TValue">
		///		The type of header value to add.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="headerName">
		///		The header name.
		/// </param>
		/// <param name="getValue">
		///		A delegate that extracts the header value from the context for each request.
		/// </param>
		/// <param name="ensureQuoted">
		///		Ensure that the header value is quoted?
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithHeader<TContext>(this HttpRequest<TContext> request, string headerName, Func<TContext, object> getValue, bool ensureQuoted = false)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(headerName));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeaderFromProvider(headerName,
				ValueProvider<TContext>.FromSelector(getValue).Convert().ValueToString(),
				ensureQuoted
			);
		}

		/// <summary>
		///		Create a copy of the request that adds an "If-Match" header to each request.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="headerValue">
		///		The header value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithIfMatchHeader<TContext>(this HttpRequest<TContext> request, string headerValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (headerValue == null)
				throw new ArgumentNullException(nameof(headerValue));

			return request.WithHeader("If-Match", () => headerValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request that adds an "If-Match" header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="getValue">
		///		A delegate that extracts the header value from the context for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithIfMatchHeader<TContext>(this HttpRequest<TContext> request, Func<TContext, string> getValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeader("If-Match", getValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request that adds an "If-Match" header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="getValue">
		///		A delegate that returns the header value for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithIfMatchHeader<TContext>(this HttpRequest<TContext> request, Func<string> getValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeader("If-Match", getValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request that adds an "If-None-Match" header to each request.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="headerValue">
		///		The header value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithIfNoneMatchHeader<TContext>(this HttpRequest<TContext> request, string headerValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (headerValue == null)
				throw new ArgumentNullException(nameof(headerValue));

			return request.WithHeader("If-None-Match", () => headerValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request that adds an "If-None-Match" header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="getValue">
		///		A delegate that extracts the header value from the context for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithIfNoneMatchHeader<TContext>(this HttpRequest<TContext> request, Func<TContext, string> getValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeader("If-None-Match", getValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request that adds an "If-None-Match" header with its value obtained from the specified delegate.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="getValue">
		///		A delegate that returns the header value for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithIfNoneMatchHeader<TContext>(this HttpRequest<TContext> request, Func<string> getValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeader("If-None-Match", getValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request that adds a header to each request.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type of object used as a context for resolving deferred parameters.
		/// </typeparam>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="headerName">
		///		The header name.
		/// </param>
		/// <param name="valueProvider">
		///		The header value provider.
		/// </param>
		/// <param name="ensureQuoted">
		///		Ensure that the header value is quoted?
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest{TContext}"/>.
		/// </returns>
		public static HttpRequest<TContext> WithHeaderFromProvider<TContext>(this HttpRequest<TContext> request, string headerName, IValueProvider<TContext, string> valueProvider, bool ensureQuoted = false)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(headerName));

			if (valueProvider == null)
				throw new ArgumentNullException(nameof(valueProvider));

			return request.WithRequestAction((requestMessage, context) =>
			{
				requestMessage.Headers.Remove(headerName);

				string headerValue = valueProvider.Get(context);
				if (headerValue == null)
					return;

				if (ensureQuoted)
					headerValue = EnsureQuoted(headerValue);

				requestMessage.Headers.Add(headerName, headerValue);
			});
		}
	}
}
