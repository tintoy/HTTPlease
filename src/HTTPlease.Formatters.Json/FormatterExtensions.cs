using Newtonsoft.Json;
using System;

namespace HTTPlease.Formatters.Json
{
	/// <summary>
	///		Extension methods for media-type formatters.
	/// </summary>
    public static class FormatterExtensions
    {
		/// <summary>
		///		Add the JSON media-type formatter.
		/// </summary>
		/// <param name="formatters">
		///		The media-type formatter collection.
		/// </param>
		/// <param name="serializerSettings">
		///		Optional settings for the JSON serialiser.
		/// </param>
		/// <returns>
		///		The media-type formatter collection (enables method-chaining).
		/// </returns>
		public static IFormatterCollection AddJsonFormatter(this IFormatterCollection formatters, JsonSerializerSettings serializerSettings = null)
		{
			if (formatters == null)
				throw new ArgumentNullException(nameof(formatters));

			formatters.Add(new JsonFormatter
			{
				SerializerSettings = serializerSettings
			});

			return formatters;
		}
    }
}
