using System;

namespace HTTPlease
{
	using Formatters.Xml;

	/// <summary>
	///		Formatter-related extension methods for <see cref="HttpRequest"/> / <see cref="HttpRequest{TContext}"/>.
	/// </summary>
	public static class XmlFormatterRequestExtensions
	{
		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, configuring it to only use the XML formatters.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest UseXml(this HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			// TODO: Clear all existing formatters, first.

			return request.WithFormatter(new XmlFormatter());
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest{TContext}"/>, configuring it to only use the XML formatters.
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
		public static HttpRequest<TContext> UseXml<TContext>(this HttpRequest<TContext> request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			// TODO: Clear all existing formatters, first.

			return request.WithFormatter(new XmlFormatter());
		}
	}
}
