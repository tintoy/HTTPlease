using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		HTTP content formatted using an <see cref="IOutputFormatter"/>.
	/// </summary>
	public class FormattedObjectContent
		: HttpContent
	{
		/// <summary>
		///		Create new formatted object content.
		/// </summary>
		/// <param name="dataType">
		///		The type of data that will be serialised to form the content.
		/// </param>
		/// <param name="data">
		///		The data that will be serialised to form the content.
		/// </param>
		/// <param name="formatter">
		///		The <see cref="IOutputFormatter"/> that will be used to serialise the data.
		/// </param>
		/// <param name="mediaType">
		///		The content type being serialised.
		/// </param>
		/// <remarks>
		///		Uses UTF-8 encoding.
		/// </remarks>
		public FormattedObjectContent(Type dataType, object data, IOutputFormatter formatter, string mediaType)
			: this(dataType, data, formatter, mediaType, Encoding.UTF8)
		{
		}

		/// <summary>
		///		Create new formatted object content.
		/// </summary>
		/// <param name="dataType">
		///		The type of data that will be serialised to form the content.
		/// </param>
		/// <param name="data">
		///		The data that will be serialised to form the content.
		/// </param>
		/// <param name="formatter">
		///		The <see cref="IOutputFormatter"/> that will be used to serialise the data.
		/// </param>
		/// <param name="mediaType">
		///		The media type that the formatter should produce.
		/// </param>
		/// <param name="encoding">
		///		The <see cref="Encoding"/> that the formatter should use for serialised data.
		/// </param>
		public FormattedObjectContent(Type dataType, object data, IOutputFormatter formatter, string mediaType, Encoding encoding)
		{
			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType));

			if (formatter == null)
				throw new ArgumentNullException(nameof(formatter));

			if (String.IsNullOrWhiteSpace(mediaType))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'contentType'.", nameof(mediaType));

			if (encoding == null)
				throw new ArgumentNullException(nameof(encoding));

			DataType = dataType;
			Data = data;
			Formatter = formatter;
			MediaType = mediaType;
			Encoding = encoding;
		}

		/// <summary>
		///		The type of data that will be serialised to form the content.
		/// </summary>
		public Type DataType { get; }

		/// <summary>
		///		The data that will be serialised to form the content.
		/// </summary>
		public object Data { get; }

		/// <summary>
		///		The <see cref="IOutputFormatter"/> that will be used to serialise the data.
		/// </summary>
		public IOutputFormatter Formatter { get; }

		/// <summary>
		///		The media type that the formatter should produce.
		/// </summary>
		public string MediaType { get; }

		/// <summary>
		///		The <see cref="Encoding"/> that the formatter should use for serialised data.
		/// </summary>
		public Encoding Encoding { get; }

		/// <summary>
		///     Try to pre-compute the formatted content length.
		/// </summary>
		/// <param name="length">
		///     The length (in bytes) of the content.
		/// 
		///		Always -1, since <see cref="FormattedObjectContent"/> length is not known before serialisation.
		/// </param>
		/// <returns>
		///     <c>false</c>.
		/// </returns>
		protected override bool TryComputeLength(out long length)
		{
			// We don't know the length in advance.
			length = -1;

			return false;
		}

		/// <summary>
		///     Serialize the HTTP content to a stream as an asynchronous operation.
		/// </summary>
		/// <returns>
		///     Returns <see cref="T:System.Threading.Tasks.Task" />.The task object representing the asynchronous operation.
		/// </returns>
		/// <param name="stream">
		///     The target stream.
		/// </param>
		/// <param name="context">
		///     Information about the transport (channel binding token, for example). This parameter may be null.
		/// </param>
		protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			OutputFormatterContext writeContext = new OutputFormatterContext(Data, DataType, MediaType, Encoding);
			await Formatter.WriteAsync(writeContext, stream);
		}
	}

	/// <summary>
	///		HTTP content formatted using an <see cref="IOutputFormatter"/>.
	/// </summary>
	public class FormattedObjectContent<T>
		: FormattedObjectContent
	{
		/// <summary>
		///		Create new formatted object content.
		/// </summary>
		/// <param name="data">
		///		The data that will be serialised to form the content.
		/// </param>
		/// <param name="formatter">
		///		The <see cref="IOutputFormatter"/> that will be used to serialise the data.
		/// </param>
		/// <param name="mediaType">
		///		The content type being serialised.
		/// </param>
		/// <remarks>
		///		Uses UTF-8 encoding.
		/// </remarks>
		public FormattedObjectContent(T data, IOutputFormatter formatter, string mediaType)
			: this(data, formatter, mediaType, Encoding.UTF8)
		{
		}

		/// <summary>
		///		Create new formatted object content.
		/// </summary>
		/// <param name="data">
		///		The data that will be serialised to form the content.
		/// </param>
		/// <param name="formatter">
		///		The <see cref="IOutputFormatter"/> that will be used to serialise the data.
		/// </param>
		/// <param name="mediaType">
		///		The media type that the formatter should produce.
		/// </param>
		/// <param name="encoding">
		///		The <see cref="FormattedObjectContent.Encoding"/> that the formatter should use for serialised data.
		/// </param>
		public FormattedObjectContent(object data, IOutputFormatter formatter, string mediaType, Encoding encoding)
			: base(typeof(T), data, formatter, mediaType, encoding)
		{
		}

		/// <summary>
		///		The data that will be serialised to form the content.
		/// </summary>
		public new T Data => (T)base.Data;
	}
}
