using System;
using System.Net;
using System.Net.Http;

namespace HTTPlease
{
	/// <summary>
	///		General-purpose extensions for <see cref="ClientBuilder"/>.
	/// </summary>
	public static class ClientFactoryExtensions
	{
		/// <summary>
		///		The <see cref="DelegatingHandler"/> CLR type.
		/// </summary>
		static readonly Type DelegatingHandlerType = typeof(DelegatingHandler);

		/// <summary>
		///		Create a new <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="clientBuilder">
		///		The HTTP client builder.
		/// </param>
		/// <param name="baseUri">
		///		The base URI for the HTTP client.
		/// </param>
		/// <param name="credentials">
		///		The client credentials used for authentication.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient CreateClient(this ClientBuilder clientBuilder, Uri baseUri, ICredentials credentials)
		{
			HttpClientHandler clientHandler = null;
			try
			{
				clientHandler = new HttpClientHandler
				{
					Credentials = credentials
				};

				return clientBuilder.CreateClient(baseUri, clientHandler);
			}
			catch (Exception)
			{
				using (clientHandler)
				{
					throw;
				}
			}
		}

		/// <summary>
		///		Create a new <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="clientBuilder">
		///		The HTTP client builder.
		/// </param>
		/// <param name="baseUri">
		///		The base URI for the HTTP client.
		/// </param>
		/// <param name="credentials">
		///		The client credentials used for authentication.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient CreateClient(this ClientBuilder clientBuilder, string baseUri, ICredentials credentials)
		{
			HttpClientHandler clientHandler = null;
			try
			{
				clientHandler = new HttpClientHandler
				{
					Credentials = credentials
				};

				return clientBuilder.CreateClient(baseUri, clientHandler);
			}
			catch (Exception)
			{
				using (clientHandler)
				{
					throw;
				}
			}
		}

		/// <summary>
		///		Create a new <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="clientBuilder">
		///		The HTTP client builder.
		/// </param>
		/// <param name="baseUri">
		///		The base URI for the HTTP client.
		/// </param>
		/// <param name="messagePipelineTerminus">
		///		An optional <see cref="HttpMessageHandler"/> that will form the message pipeline terminus.
		/// 
		/// 	If not specified, a new <see cref="HttpClientHandler"/> is used.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient CreateClient(this ClientBuilder clientBuilder, string baseUri, HttpMessageHandler messagePipelineTerminus = null)
		{
			if (clientBuilder == null)
				throw new ArgumentNullException(nameof(clientBuilder));

			if (String.IsNullOrWhiteSpace(baseUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'baseUri'.", nameof(baseUri));

			return clientBuilder.CreateClient(
				new Uri(baseUri, UriKind.Absolute),
				messagePipelineTerminus
			);
		}

		/// <summary>
		///		Register a message handler type for the pipeline used by clients created by the factory.
		/// </summary>
		/// <typeparam name="TMessageHandler">
		///		The message handler type.
		/// 
		///		Must be a sub-type of <see cref="DelegatingHandler"/> (not <see cref="DelegatingHandler"/> itself).
		/// </typeparam>
		/// <param name="clientFactory">
		///		The client factory.
		/// </param>
		/// <returns>
		///		The client factory (enables inline use / method chaining).
		/// </returns>
		public static ClientBuilder WithMessageHandler<TMessageHandler>(this ClientBuilder clientFactory)
			where TMessageHandler : DelegatingHandler, new()
		{
			if (clientFactory == null)
				throw new ArgumentNullException(nameof(clientFactory));

			if (typeof(TMessageHandler) == DelegatingHandlerType)
				throw new NotSupportedException("TMessageHandler must be a sub-type of DelegatingHandler (it cannot be DelegatingHandler).");

			clientFactory.AddHandler(
				() => new TMessageHandler()
			);

			return clientFactory;
		}
	}
}
