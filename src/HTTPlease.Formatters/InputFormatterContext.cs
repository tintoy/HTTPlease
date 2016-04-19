using System;
using System.Text;

namespace HTTPlease.Formatters
{
	using System.IO;

	/// <summary>
	///		Contextual information used by input formatters.
	/// </summary>
	public class InputFormatterContext
	{
		/// <summary>
		///		Create a new <see cref="InputFormatterContext"/>.
		/// </summary>
		/// <param name="dataType">
		///		The CLR type into which the data will be deserialised.
		/// </param>
		/// <param name="contentType">
		///		The content type being deserialised.
		/// </param>
		/// <param name="encoding">
		///		The content encoding.
		/// </param>
		public InputFormatterContext(Type dataType, string contentType, Encoding encoding)
		{
			DataType = dataType;
			ContentType = contentType;
			Encoding = encoding;
		}

		/// <summary>
		///		The CLR type into which the data will be deserialised.
		/// </summary>
		public Type DataType { get; }

		/// <summary>
		///		The content type being deserialised.
		/// </summary>
		public string ContentType { get; }

		/// <summary>
		///		The content encoding.
		/// </summary>
		public Encoding Encoding { get; }

		/// <summary>
		///		Create a <see cref="TextReader"/> from the specified input stream.
		/// </summary>
		/// <param name="inputStream">
		///		The input stream.
		/// </param>
		/// <returns>
		///		The <see cref="TextReader"/>.
		/// </returns>
		/// <remarks>
		///		The <see cref="TextReader"/>, when closed, will not close the input stream.
		/// </remarks>
		public virtual TextReader CreateReader(Stream inputStream)
		{
			if (inputStream == null)
				throw new ArgumentNullException(nameof(inputStream));

			return StreamHelper.CreateTransientTextReader(inputStream, Encoding);
		}
	}
}