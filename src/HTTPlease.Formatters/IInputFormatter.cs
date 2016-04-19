using System;
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
		///		Determine whether the formatter can deserialise content of the specified type into the specified data type.
		/// </summary>
		/// <param name="dataType">
		///		The CLR type that the formatter will deserialise.
		/// </param>
		/// <param name="contentType">
		///		The content type.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the formatter can deserialise data into the specified target type; otherwise, <c>false</c>.
		/// </returns>
		bool CanReadType(Type dataType, string contentType);

		/// <summary>
		///		Asynchronously deserialise data from an input stream.
		/// </summary>
		/// <param name="dataType">
		///		The CLR type to deserialise.
		/// </param>
		/// <param name="stream">
		///		The input stream from which to read serialised data.
		/// </param>
		/// <param name="contentType">
		///		The stream content type.
		/// </param>
		/// <returns>
		///		The deserialised object.
		/// </returns>
		Task<object> ReadAsync(Stream stream, Type dataType, string contentType);
	}
}
