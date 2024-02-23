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
		///		Configure an <see cref="IHttpClientBuilder"/> to use an HTTPlease <see cref="ClientBuilder"/> for handler configuration.
		/// </summary>
		/// <param name="client">
		///		The <see cref="IHttpClientBuilder"/> to configure.
		/// </param>
		/// <param name="httplease">
		///		The HTTPlease <see cref="ClientBuilder"/> to use for handler configuration.
		/// </param>
		/// <param name="includePipelineTerminus">
		///		Also use the <see cref="ClientBuilder"/> to create the handler pipeline?
		///		
		///		NOTE: under normal circumstances, this probably is not what you want (Microsoft.Extensions.Http usually provides a pipeline terminus that takes are of connection pooling, etc).
		/// </param>
		/// <returns>
		///		The configured <see cref="IHttpClientBuilder"/>.
		/// </returns>
		public static IHttpClientBuilder UseHTTPlease(this IHttpClientBuilder client, ClientBuilder httplease, bool includePipelineTerminus = false)
		{
			if (client == null)
				throw new ArgumentNullException(nameof(client));

			if (httplease == null)
				throw new ArgumentNullException(nameof(httplease));

			if (includePipelineTerminus)
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
		///		Configure an <see cref="IHttpClientBuilder"/> to use an HTTPlease <see cref="ClientBuilder{TContext}"/> for handler configuration.
		/// </summary>
		/// <param name="client">
		///		The <see cref="IHttpClientBuilder"/> to configure.
		/// </param>
		/// <param name="httplease">
		///		The HTTPlease <see cref="ClientBuilder{TContext}"/> to use for handler configuration.
		///		
		///		<para>
		///			 Configuration delegates will have access to a scoped <see cref="IServiceProvider"/>; see <see href="https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#message-handler-scopes-in-ihttpclientfactory"/> for further information.
		///		</para>
		/// </param>
		/// <param name="includePipelineTerminus">
		///		Also use the <see cref="ClientBuilder"/> to create the handler pipeline?
		///		
		///		NOTE: under normal circumstances, this probably is not what you want (Microsoft.Extensions.Http usually provides a pipeline terminus that takes are of connection pooling, etc).
		/// </param>
		/// <returns>
		///		The configured <see cref="IHttpClientBuilder"/>.
		/// </returns>
		public static IHttpClientBuilder UseHTTPlease(this IHttpClientBuilder client, ClientBuilder<IServiceProvider> httplease, bool includePipelineTerminus = false)
		{
			if (client == null)
				throw new ArgumentNullException(nameof(client));

			if (httplease == null)
				throw new ArgumentNullException(nameof(httplease));

			if (includePipelineTerminus)
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
