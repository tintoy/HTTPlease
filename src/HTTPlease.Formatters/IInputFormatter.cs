using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		Represents a facility for deserialising data for one or more media types.
	/// </summary>
    public interface IInputFormatter
	{
		/// <summary>
		///		Content types supported by the formatter.
		/// </summary>
		ISet<string> SupportedContentTypes { get; }

		/// <summary>
		///		Determine whether the formatter can deserialise the specified data.
		/// </summary>
		/// <param name="context">
		///		Contextual information about the data being deserialised.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the formatter can deserialise the data; otherwise, <c>false</c>.
		/// </returns>
		bool CanReadType(InputFormatterContext context);

		/// <summary>
		///		Asynchronously deserialise data from an input stream.
		/// </summary>
		/// <param name="context">
		///		Contextual information about the data being deserialised.
		/// </param>
		/// <param name="stream">
		///		The input stream from which to read serialised data.
		/// </param>
		/// <returns>
		///		The deserialised object.
		/// </returns>
		Task<object> ReadAsync(InputFormatterContext context, Stream stream);
	}
}
