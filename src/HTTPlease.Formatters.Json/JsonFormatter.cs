using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HTTPlease.Formatters.Json
{
	/// <summary>
	///		Media-type formatter for JSON.
	/// </summary>
	public class JsonFormatter
		: IInputFormatter, IOutputFormatter
	{
		/// <summary>
		///		The standard JSON content type supported by the formatter.
		/// </summary>
		public static readonly string JsonContentType = "application/json";

		/// <summary>
		///		Create a new <see cref="JsonFormatter"/>.
		/// </summary>
		public JsonFormatter()
		{
		}

		/// <summary>
		///		Settings for the JSON serialiser.
		/// </summary>
		public JsonSerializerSettings SerializerSettings { get; set; }

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
		public bool CanReadType(Type dataType, string contentType)
		{
			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType));

			if (String.IsNullOrWhiteSpace(contentType))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'contentType'.", nameof(contentType));

			return contentType == JsonContentType;
		}

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
		public bool CanWrite(Type dataType, string contentType)
		{
			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType));

			if (String.IsNullOrWhiteSpace(contentType))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'contentType'.", nameof(contentType));

			return contentType == JsonContentType;
		}

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
		public Task<object> ReadAsync(Stream stream, Type dataType, string contentType)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType));

			if (String.IsNullOrWhiteSpace(contentType))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'contentType'.", nameof(contentType));

			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true))
			{
				JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
				object data = serializer.Deserialize(reader, dataType);

				return Task.FromResult(data);
			}
		}

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
		public Task WriteAsync(object data, Stream stream, Type dataType, string contentType)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType));

			if (String.IsNullOrWhiteSpace(contentType))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'contentType'.", nameof(contentType));

			if (contentType != JsonContentType)
				throw new NotSupportedException($"The {nameof(JsonFormatter)} cannot write content of type '{contentType}'.");

			using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 1024, leaveOpen: true))
			{
				JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
				serializer.Serialize(writer, data, dataType);
			}

			return Task.CompletedTask;
		}
	}
}
