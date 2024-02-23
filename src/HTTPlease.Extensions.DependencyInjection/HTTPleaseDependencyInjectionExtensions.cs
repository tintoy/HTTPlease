using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;

// Root namespace, for discoverability.
namespace HTTPlease
{
	/// <summary>
	///		Extension methods for registering and resolving HTTPlease components for dependency injection.
	/// </summary>
	public static class HTTPleaseDependencyInjectionExtensions
	{
		/// <summary>
		///		Register a named <see cref="HttpClient"/> (via <see cref="IHttpClientFactory"/>) using the configuration from an HTTPlease <see cref="ClientBuilder"/>.
		/// </summary>
		/// <param name="services">
		///		The <see cref="IServiceCollection"/> to configure.
		/// </param>
		/// <param name="name">
		///		The name of the <see cref="HttpClient"/> configuration to register.
		/// </param>
		/// <param name="httplease">
		///		The HTTPlease <see cref="ClientBuilder"/> to use for handler configuration.
		/// </param>
		/// <returns>
		///		An <see cref="IHttpClientBuilder"/> representing the <see cref="HttpClient"/> configuration (enables further customisation).
		/// </returns>
		public static IHttpClientBuilder AddHttpClient(this IServiceCollection services, string name, ClientBuilder httplease)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException($"Argument cannot be null, empty, or entirely composed of whitespace: {nameof(name)}.", nameof(name));

			if (httplease == null)
				throw new ArgumentNullException(nameof(httplease));

			IHttpClientBuilder client = services.AddHttpClient(name);

			return client.Configure(httplease);
		}

		/// <summary>
		///		Register a named <see cref="HttpClient"/> (via <see cref="IHttpClientFactory"/>) using the configuration from an HTTPlease <see cref="ClientBuilder"/>.
		/// </summary>
		/// <param name="services">
		///		The <see cref="IServiceCollection"/> to configure.
		/// </param>
		/// <param name="name">
		///		The name of the <see cref="HttpClient"/> configuration to register.
		/// </param>
		/// <param name="httplease">
		///		The HTTPlease <see cref="ClientBuilder{TContext}"/> to use for handler configuration.
		///		
		///		<para>
		///			 Configuration delegates will have access to a scoped <see cref="IServiceProvider"/>; see <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#message-handler-scopes-in-ihttpclientfactory"/> for further information.
		///		</para>
		/// </param>
		/// <returns>
		///		An <see cref="IHttpClientBuilder"/> representing the <see cref="HttpClient"/> configuration (enables further customisation).
		/// </returns>
		public static IHttpClientBuilder AddHttpClient(this IServiceCollection services, string name, ClientBuilder<IServiceProvider> httplease)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException($"Argument cannot be null, empty, or entirely composed of whitespace: {nameof(name)}.", nameof(name));

			if (httplease == null)
				throw new ArgumentNullException(nameof(httplease));

			IHttpClientBuilder client = services.AddHttpClient(name);

			return client.Configure(httplease);
		}

		/// <summary>
		///		Configure an <see cref="HttpClient"/> using an HTTPlease client-builder.
		/// </summary>
		/// <param name="client">
		///		An <see cref="IHttpClientBuilder"/> representing the <see cref="HttpClient"/> configuration.
		/// </param>
		/// <param name="httplease">
		///		The HTTPlease <see cref="ClientBuilder"/> used to configure the <see cref="HttpClient"/>.
		/// </param>
		/// <returns>
		///		The configured <see cref="IHttpClientBuilder"/>.
		/// </returns>
		static IHttpClientBuilder Configure(this IHttpClientBuilder client, ClientBuilder httplease)
		{
			if (client == null)
				throw new ArgumentNullException(nameof(client));

			if (httplease == null)
				throw new ArgumentNullException(nameof(httplease));

			if (httplease.HasCustomPipelineTerminus)
			{
				client.ConfigurePrimaryHttpMessageHandler(
					() => httplease.BuildPipelineTerminus()
				);
			}

			client.ConfigureAdditionalHttpMessageHandlers((handlers, serviceProvider) =>
			{
				List<DelegatingHandler> httpleaseHandlers = httplease.CreatePipelineHandlers();

				foreach (DelegatingHandler httpleaseHandler in httpleaseHandlers)
					handlers.Add(httpleaseHandler);
			});

			return client;
		}

		/// <summary>
		///		Configure an <see cref="HttpClient"/> using an HTTPlease client-builder.
		/// </summary>
		/// <param name="client">
		///		An <see cref="IHttpClientBuilder"/> representing the <see cref="HttpClient"/> configuration.
		/// </param>
		/// <param name="httplease">
		///		The HTTPlease <see cref="ClientBuilder{TContext}"/> used to configure the <see cref="HttpClient"/>.
		/// </param>
		/// <returns>
		///		The configured <see cref="IHttpClientBuilder"/>.
		/// </returns>
		static IHttpClientBuilder Configure(this IHttpClientBuilder client, ClientBuilder<IServiceProvider> httplease)
		{
			if (client == null)
				throw new ArgumentNullException(nameof(client));

			if (httplease == null)
				throw new ArgumentNullException(nameof(httplease));

			if (httplease.HasCustomPipelineTerminus)
			{
				client.ConfigurePrimaryHttpMessageHandler(
					serviceProvider => httplease.BuildPipelineTerminus(serviceProvider)
				);
			}

			client.ConfigureAdditionalHttpMessageHandlers((handlers, serviceProvider) =>
			{
				List<DelegatingHandler> httpleaseHandlers = httplease.CreatePipelineHandlers(serviceProvider);

				foreach (DelegatingHandler httpleaseHandler in httpleaseHandlers)
					handlers.Add(httpleaseHandler);
			});

			return client;
		}
	}
}
