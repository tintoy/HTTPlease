using System;
using System.Collections.Immutable;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		Extension methods for working with <see cref="HttpRequest"/>s.
	/// </summary>
    public static class RequestExtensions
    {
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
		public static HttpRequest WithFormatter(this HttpRequest request, IFormatter formatter)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			Type formatterType = formatter.GetType();

			request.Clone(properties =>
			{
				ImmutableDictionary<Type, IFormatter> formatters = request.GetFormatters();

				// If this is the first formatter we're adding, then make sure that we'll populate the formatter collection for each outgoing request.
				if (formatters.Count == 0)
				{
					properties[nameof(request.RequestActions)] = request.RequestActions.Add((requestMessage, context) =>
					{
						requestMessage.Properties[MessageProperties.MediaTypeFormatters] = new FormatterCollection(formatters.Values);
					});
				}

				properties[MessageProperties.MediaTypeFormatters] = formatters.SetItem(formatterType, formatter);
			});

			return request;
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
		public static HttpRequest WithoutFormatter(this HttpRequest request, Type formatterType)
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
			
			request.Clone(properties =>
			{
				properties[MessageProperties.MediaTypeFormatters] = formatters.Remove(formatterType);
			});

			return request;
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
		public static ImmutableDictionary<Type, IFormatter> GetFormatters(this HttpRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			object formatters;
			if (request.Properties.TryGetValue(MessageProperties.MediaTypeFormatters, out formatters))
				return (ImmutableDictionary<Type, IFormatter>)formatters;

			return ImmutableDictionary<Type, IFormatter>.Empty;
		}
	}
}
