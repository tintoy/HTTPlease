using System;
using System.Collections.Generic;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		Represents a collection of <see cref="IInputFormatter"/>s.
	/// </summary>
    public interface IMediaTypeFormatters
		: ICollection<IInputFormatter>
	{
		/// <summary>
		///		Get the most appropriate <see cref="IInputFormatter">formatter</see> to read the specified data type from the specified content type.
		/// </summary>
		/// <param name="dataType">
		///		The CLR type into which the data will be deserialised.
		/// </param>
		/// <param name="contentType">
		///		The content type.
		/// </param>
		/// <returns>
		///		The formatter, or <c>null</c> if none of the formatters in the collection can handle the specified content type.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="dataType"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The specified content type is <c>null</c>, empty, or entirely composed of whitespace.
		/// </exception>
		IInputFormatter FindInputFormatter(Type dataType, string contentType);

		/// <summary>
		///		Find the most appropriate <see cref="IOutputFormatter">formatter</see> to write the specified data type to the specified content type.
		/// </summary>
		/// <param name="dataType">
		///		The CLR type whose data will be serialised.
		/// </param>
		/// <param name="contentType">
		///		The content type.
		/// </param>
		/// <returns>
		///		The formatter, or <c>null</c> if none of the formatters in the collection can handle the specified content type.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="dataType"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		The specified content type is <c>null</c>, empty, or entirely composed of whitespace.
		/// </exception>
		IOutputFormatter FindOutputFormatter(Type dataType, string contentType);
	}
}
