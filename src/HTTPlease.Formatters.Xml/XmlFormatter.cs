using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace HTTPlease.Formatters.Xml
{
	/// <summary>
	///		Content formatter for XML.
	/// </summary>
	/// <remarks>
	///		Uses <see cref="DataContractSerializer"/>, so for CoreCLR you'll need to reference System.Runtime.Serialization.Primitives.
	/// </remarks>
	public class XmlFormatter
		: IInputFormatter, IOutputFormatter
	{
		/// <summary>
		///		Create a new <see cref="XmlFormatter"/>.
		/// </summary>
		public XmlFormatter()
		{
		}

		/// <summary>
		///		Content types supported by the formatter.
		/// </summary>
		public ISet<string> SupportedMediaTypes { get; } = new HashSet<string> { WellKnownMediaTypes.Xml };

		/// <summary>
		///		Determine whether the formatter can deserialise the specified data.
		/// </summary>
		/// <param name="context">
		///		Contextual information about the data being deserialised.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the formatter can deserialise the data; otherwise, <c>false</c>.
		/// </returns>
		public bool CanRead(InputFormatterContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			return SupportedMediaTypes.Contains(context.MediaType);
		}

		/// <summary>
		///		Determine whether the formatter can serialise the specified data.
		/// </summary>
		/// <param name="context">
		///		Contextual information about the data being serialised.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the formatter can serialise the data; otherwise, <c>false</c>.
		/// </returns>
		public bool CanWrite(OutputFormatterContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			return SupportedMediaTypes.Contains(context.MediaType);
		}

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
		public Task<object> ReadAsync(InputFormatterContext context, Stream stream)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			using (XmlReader reader = XmlReader.Create(stream))
			{
				DataContractSerializer serializer = new DataContractSerializer(context.DataType);
				object data = serializer.ReadObject(reader);

				return Task.FromResult(data);
			}
		}

		/// <summary>
		///		Asynchronously serialise data to an output stream.
		/// </summary>
		/// <param name="context">
		///		Contextual information about the data being deserialised.
		/// </param>
		/// <param name="stream">
		///		The output stream to which the serialised data will be written.
		/// </param>
		/// <returns>
		///		A <see cref="Task"/> representing the asynchronous operation.
		/// </returns>
		public Task WriteAsync(OutputFormatterContext context, Stream stream)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			if (!SupportedMediaTypes.Contains(context.MediaType))
				throw new NotSupportedException($"The {nameof(XmlFormatter)} cannot write content of type '{context.MediaType}'.");

			using (XmlWriter writer = XmlWriter.Create(stream))
			{
				DataContractSerializer serializer = new DataContractSerializer(context.DataType);
				serializer.WriteObject(writer, context.Data);

				return Task.CompletedTask;
			}
		}
	}
}
