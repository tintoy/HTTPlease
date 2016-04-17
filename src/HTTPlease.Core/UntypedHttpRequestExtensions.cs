using System;
using System.Text;

namespace HTTPlease
{
	using ValueProviders;

	/// <summary>
	///		Extension methods for <see cref="IHttpRequest"/>.
	/// </summary>
    public static class UntypedHttpRequestExtensions
    {
		#region Absolute URIs

		/// <summary>
		///		Ensure that the <see cref="IHttpRequest"/> has an <see cref="UriKind.Absolute">absolute</see> <see cref="Uri">URI</see>.
		/// </summary>
		/// <returns>
		///		The request's absolute URI.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The request has a <see cref="UriKind.Relative">relative</see> <see cref="Uri">URI</see>.
		/// </exception>
		public static bool HasAbsoluteUri(this IHttpRequest httpRequest)
		{
			if (httpRequest == null)
				throw new ArgumentNullException(nameof(httpRequest));

			return httpRequest.RequestUri.IsAbsoluteUri;
		}

		/// <summary>
		///		Ensure that the <see cref="IHttpRequest"/> has an <see cref="UriKind.Absolute">absolute</see> <see cref="Uri">URI</see>.
		/// </summary>
		/// <returns>
		///		The request's absolute URI.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The request has a <see cref="UriKind.Relative">relative</see> <see cref="Uri">URI</see>.
		/// </exception>
		public static Uri EnsureAbsoluteUri(this IHttpRequest httpRequest)
		{
			if (httpRequest == null)
				throw new ArgumentNullException(nameof(httpRequest));

			Uri requestUri = httpRequest.RequestUri;
			if (requestUri.IsAbsoluteUri)
				return requestUri;

			throw new InvalidOperationException("The HTTP request does not have an absolute URI.");
		}

		#endregion // Absolute URIs

		#region Headers

		/// <summary>
		///		Create a copy of the request builder that adds a header to each request.
		/// </summary>
		/// <param name="request">
		///		The HTTP request builder.
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
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithHeader(this HttpRequest request, string headerName, string headerValue, bool ensureQuoted = false)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(headerName));

			if (headerValue == null)
				throw new ArgumentNullException(nameof(headerValue));

			return request.WithHeaderFromProvider(headerName,
				SimpleValueProvider.FromConstantValue(headerValue),
				ensureQuoted
			);
		}

		/// <summary>
		///		Create a copy of the request builder that adds a header with its value obtained from the specified delegate.
		/// </summary>
		/// <param name="request">
		///		The HTTP request builder.
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
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithHeader<TValue>(this HttpRequest request, string headerName, Func<TValue> getValue, bool ensureQuoted = false)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(headerName));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeaderFromProvider(headerName,
				SimpleValueProvider.FromFunction(getValue).Convert().ValueToString(),
				ensureQuoted
			);
		}

		/// <summary>
		///		Create a copy of the request builder that adds an "If-Match" header to each request.
		/// </summary>
		/// <param name="request">
		///		The HTTP request builder.
		/// </param>
		/// <param name="headerValue">
		///		The header value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithIfMatchHeader(this HttpRequest request, string headerValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (headerValue == null)
				throw new ArgumentNullException(nameof(headerValue));

			return request.WithHeader("If-Match", () => headerValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request builder that adds an "If-Match" header with its value obtained from the specified delegate.
		/// </summary>
		/// <param name="request">
		///		The HTTP request builder.
		/// </param>
		/// <param name="getValue">
		///		A delegate that returns the header value for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithIfMatchHeader(this HttpRequest request, Func<string> getValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeader("If-Match", getValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request builder that adds an "If-None-Match" header to each request.
		/// </summary>
		/// <param name="request">
		///		The HTTP request builder.
		/// </param>
		/// <param name="headerValue">
		///		The header value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithIfNoneMatchHeader(this HttpRequest request, string headerValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (headerValue == null)
				throw new ArgumentNullException(nameof(headerValue));

			return request.WithHeader("If-None-Match", () => headerValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request builder that adds an "If-None-Match" header with its value obtained from the specified delegate.
		/// </summary>
		/// <param name="request">
		///		The HTTP request builder.
		/// </param>
		/// <param name="getValue">
		///		A delegate that returns the header value for each request.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithIfNoneMatchHeader(this HttpRequest request, Func<string> getValue)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return request.WithHeader("If-None-Match", getValue, ensureQuoted: true);
		}

		/// <summary>
		///		Create a copy of the request builder that adds a header to each request.
		/// </summary>
		/// <param name="request">
		///		The HTTP request builder.
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
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest WithHeaderFromProvider(this HttpRequest request, string headerName, ISimpleValueProvider<string> valueProvider, bool ensureQuoted = false)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (String.IsNullOrWhiteSpace(headerName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(headerName));

			if (valueProvider == null)
				throw new ArgumentNullException(nameof(valueProvider));

			return request.WithRequestAction(requestMessage =>
			{
				requestMessage.Headers.Remove(headerName);

				string headerValue = valueProvider.Get();
				if (headerValue == null)
					return;

				if (ensureQuoted)
					headerValue = EnsureQuoted(headerValue);

				requestMessage.Headers.Add(headerName, headerValue);
			});
		}

		#endregion // Headers

		#region Helpers

		/// <summary>
		///		Ensure that the specified string is surrounted by quotes.
		/// </summary>
		/// <param name="str">
		///		The string to examine.
		/// </param>
		/// <returns>
		///		The string, with quotes prepended / appended as required.
		/// </returns>
		/// <remarks>
		///		Some HTTP headers (such as If-Match) require their values to be quoted.
		/// </remarks>
		static string EnsureQuoted(string str)
		{
			if (str == null)
				throw new ArgumentNullException(nameof(str));

			if (str.Length == 0)
				return "\"\"";

			StringBuilder quotedStringBuilder = new StringBuilder(str);

			if (quotedStringBuilder[0] != '\"')
				quotedStringBuilder.Insert(0, '\"');

			if (quotedStringBuilder[quotedStringBuilder.Length - 1] != '\"')
				quotedStringBuilder.Append('\"');

			return quotedStringBuilder.ToString();
		}

		#endregion // Helpers
	}
}

