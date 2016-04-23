namespace HTTPlease
{
	/// <summary>
	///		The names of well-known HttpRequestMessage / HttpResponseMessage properties.
	/// </summary>
    public static class MessageProperties
	{
		/// <summary>
		///		The prefix for HTTPlease property names.
		/// </summary>
		static string Prefix = "HTTPlease.";

		/// <summary>
		///		The <see cref="IHttpRequest"/> that created the message.
		/// </summary>
		public static string Request = Prefix + "Request";

		/// <summary>
		///		The message's collection of content formatters.
		/// </summary>
		public static string ContentFormatters = Prefix + "ContentFormatters";
	}
}
