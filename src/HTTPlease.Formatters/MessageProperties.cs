using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		The names of well-known <see cref="HttpRequestMessage"/> / <see cref="HttpResponseMessage"/> properties.
	/// </summary>
    public static class MessageProperties
	{
		/// <summary>
		///		The prefix for HTTPlease property names.
		/// </summary>
		static string Prefix = "HTTPlease.";

		/// <summary>
		///		Media-type formatters.
		/// </summary>
		public static string MediaTypeFormatters = Prefix + "MediaTypeFormatters";
	}
}
