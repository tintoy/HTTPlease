using System;

namespace HTTPlease
{
	/// <summary>
	///		Extension methods for <see cref="IHttpRequest"/>.
	/// </summary>
    public static class HttpRequestExtensions
    {
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
	}
}

