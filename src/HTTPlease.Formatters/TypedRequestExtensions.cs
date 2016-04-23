﻿using System;
using System.Collections.Immutable;
using System.Net.Http;
using System.Text;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		Extension methods for working with <see cref="HttpRequest"/>s.
	/// </summary>
	public static class TypedRequestExtensions
    {
		/// <summary>
		///		Build an HTTP request message, selecting an appropriate content formatter to serialise its body content.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <param name="httpMethod">
		///		The HTTP request method.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance to use as a context for resolving deferred template parameters.
		/// </param>
		/// <param name="bodyContent">
		///		The request body content.
		/// </param>
		/// <param name="mediaType">
		///		The request body media type to use.
		/// </param>
		/// <param name="baseUri">
		///		An optional base URI to use if the request does not already have an absolute request URI.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpRequestMessage"/>.
		/// </returns>
		public static HttpRequestMessage BuildRequestMessage<TContext>(this HttpRequest<TContext> request, HttpMethod httpMethod, TContext context, object bodyContent, string mediaType, Uri baseUri = null)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return request.BuildRequestMessage(httpMethod, context, bodyContent, mediaType, Encoding.UTF8, baseUri);
		}

		/// <summary>
		///		Build an HTTP request message, selecting an appropriate content formatter to serialise its body content.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <param name="httpMethod">
		///		The HTTP request method.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance to use as a context for resolving deferred template parameters.
		/// </param>
		/// <param name="bodyContent">
		///		The request body content.
		/// </param>
		/// <param name="mediaType">
		///		The request body media type to use.
		/// </param>
		/// <param name="encoding">
		///		The request body encoding to use.
		/// </param>
		/// <param name="baseUri">
		///		An optional base URI to use if the request does not already have an absolute request URI.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpRequestMessage"/>.
		/// </returns>
		public static HttpRequestMessage BuildRequestMessage<TContext>(this HttpRequest<TContext> request, HttpMethod httpMethod, TContext context, object bodyContent, string mediaType, Encoding encoding, Uri baseUri = null)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			HttpContent httpContent = bodyContent as HttpContent;
			if (httpContent == null && bodyContent != null)
			{
				IFormatterCollection formatters = request.CreateFormatterCollection();

				OutputFormatterContext writeContext = new OutputFormatterContext(bodyContent, bodyContent.GetType(), mediaType, encoding);
				IOutputFormatter writeFormatter = formatters.FindOutputFormatter(writeContext);
				if (writeFormatter == null)
					throw new HttpRequestException($"None of the supplied formatters can write data of type '{writeContext.DataType.FullName}' to media type '{writeContext.MediaType}'.");

				httpContent = new FormattedObjectContent(writeFormatter, writeContext);
			}

			return request.BuildRequestMessage(httpMethod, context, httpContent, baseUri);
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, adding the specified content formatter.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <param name="formatter">
		///		The content formatter to add.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest<TContext> WithFormatter<TContext>(this HttpRequest<TContext> request, IFormatter formatter)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			Type formatterType = formatter.GetType();

			return request.Clone(properties =>
			{
				ImmutableDictionary<Type, IFormatter> formatters = request.GetFormatters();

				// If this is the first formatter we're adding, then make sure that we'll populate the formatter collection for each outgoing request.
				if (formatters.Count == 0)
				{
					properties[nameof(request.RequestActions)] = request.RequestActions.Add((requestMessage, context) =>
					{
						requestMessage.Properties[MessageProperties.ContentFormatters] = new FormatterCollection(formatters.Values);
					});
				}

				properties[MessageProperties.ContentFormatters] = formatters.SetItem(formatterType, formatter);
			});
		}

		/// <summary>
		///		Create a copy of the <see cref="HttpRequest"/>, adding the specified content formatter.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <param name="formatterType">
		///		The type of content formatter to remove.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest<TContext> WithoutFormatter<TContext>(this HttpRequest<TContext> request, Type formatterType)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (formatterType == null)
				throw new ArgumentNullException(nameof(formatterType));

			ImmutableDictionary<Type, IFormatter> formatters = request.GetFormatters();
			if (formatters == null)
				return request;

			if (!formatters.ContainsKey(formatterType))
				return request;
			
			return request.Clone(properties =>
			{
				properties[MessageProperties.ContentFormatters] = formatters.Remove(formatterType);
			});
		}

		/// <summary>
		///		Get the collection formatters used by the <see cref="HttpRequest"/>.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <returns>
		///		An immutable dictionary of formatters, keyed by type.
		/// </returns>
		public static ImmutableDictionary<Type, IFormatter> GetFormatters<TContext>(this HttpRequest<TContext> request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			object formatters;
			if (request.Properties.TryGetValue(MessageProperties.ContentFormatters, out formatters))
				return (ImmutableDictionary<Type, IFormatter>)formatters;

			return ImmutableDictionary<Type, IFormatter>.Empty;
		}

		/// <summary>
		///		Create an <see cref="IFormatterCollection"/> from the request's registered formatters.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <returns>
		///		An <see cref="IFormatterCollection"/> representing the formatter collection.
		/// </returns>
		public static IFormatterCollection CreateFormatterCollection<TContext>(this HttpRequest<TContext> request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new FormatterCollection(
				request.GetFormatters().Values
			);
		}
	}
}
