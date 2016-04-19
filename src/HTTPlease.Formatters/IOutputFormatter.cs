using System;
using System.IO;
using System.Threading.Tasks;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		Represents a facility for serialising data to one or more media types.
	/// </summary>
    public interface IOutputFormatter
    {
		/// <summary>
		///		Determine whether the formatter can serialise data from the specified type.
		/// </summary>
		/// <param name="dataType">
		///		The CLR type that the formatter will serialise.
		/// </param>
		/// <param name="contentType">
		///		The content type to which the data will be serialised.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the formatter can serialise data from the specified source type into the specified content type; otherwise, <c>false</c>.
		/// </returns>
		bool CanWrite(Type dataType, string contentType);

		/// <summary>
		///		Asynchronously serialise data to an output stream.
		/// </summary>
		/// <param name="data">
		///		The data to serialise.
		/// </param>
		/// <param name="dataType">
		///		The CLR type to serialise.
		/// </param>
		/// <param name="stream">
		///		The output stream to which the serialised data will be written.
		/// </param>
		/// <param name="contentType">
		///		The stream content type.
		/// </param>
		/// <returns>
		///		A <see cref="Task"/> representing the asynchronous operation.
		/// </returns>
		Task WriteAsync(object data, Stream stream, Type dataType, string contentType);
    }
}
