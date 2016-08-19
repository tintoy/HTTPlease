using System;
using System.Net.Http;

namespace HTTPlease.Security
{
    using Abstractions;
    using MessageHandlers;

    /// <summary>
    ///		Security-related extension methods for the <see cref="ClientBuilder">HTTP client builder</see>.
    /// </summary>
    public static class ClientBuilderExtensions
    {
		/// <summary>
		///		Create a copy of the <see cref="ClientBuilder"/> with transparent authentication support for its clients.
		/// </summary>
		/// <param name="clientBuilder">
		///		The HTTP client builder.
		/// </param>
		/// <param name="authenticationProvider">
		///		The <see cref="IHttpRequestAuthenticationProvider"/> used to add authentication to outgoing requests.
		/// </param>
		/// <returns>
		///		The new HTTP client builder.
		/// </returns>
		/// <remarks>
		///		In general, this overload should only be used in test scenarios (or where you will only ever have a single HTTP client).
		///		It is not good practice to share an <see cref="IHttpRequestAuthenticationProvider"/> between multiple clients.
		/// </remarks>
		public static ClientBuilder WithAuthentication(this ClientBuilder clientBuilder, IHttpRequestAuthenticationProvider authenticationProvider)
		{
			if (clientBuilder == null)
				throw new ArgumentNullException(nameof(clientBuilder));

			if (authenticationProvider == null)
				throw new ArgumentNullException(nameof(authenticationProvider));

			return clientBuilder.WithAuthentication(() => authenticationProvider);
		}

		/// <summary>
		///		Create a copy of the <see cref="ClientBuilder"/> with transparent authentication support for its clients.
		/// </summary>
		/// <param name="clientBuilder">
		///		The HTTP client builder.
		/// </param>
		/// <param name="authenticationProviderFactory">
		///		A///  delegate that creates a new <see cref="IHttpRequestAuthenticationProvider"/> for each <see cref="HttpClient"/> produced by the <see cref="ClientBuilder"/>.
		/// </param>
		/// <returns>
		///		The new HTTP client builder.
		/// </returns>
		public static ClientBuilder WithAuthentication(this ClientBuilder clientBuilder, Func<IHttpRequestAuthenticationProvider> authenticationProviderFactory)
		{
			if (clientBuilder == null)
				throw new ArgumentNullException(nameof(clientBuilder));

			if (authenticationProviderFactory == null)
				throw new ArgumentNullException(nameof(authenticationProviderFactory));

			return clientBuilder.AddHandler(() =>
			{
				IHttpRequestAuthenticationProvider authenticationProvider = authenticationProviderFactory();

				return new AuthenticationMessageHandler(authenticationProvider);
			});
		}
	}
}
