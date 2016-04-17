using System;
using System.Net.Http;

namespace HTTPlease.Clients
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
		/// <returns>
		///		The new <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient CreateClient(this ClientBuilder clientBuilder, string baseUri)
		{
			if (clientBuilder == null)
				throw new ArgumentNullException(nameof(clientBuilder));

			if (String.IsNullOrWhiteSpace(baseUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'baseUri'.", nameof(baseUri));

			return clientBuilder.CreateClient(
				new Uri(baseUri, UriKind.Absolute)
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
