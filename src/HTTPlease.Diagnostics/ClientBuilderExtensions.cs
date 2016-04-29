using Microsoft.Extensions.Logging;
using System;

namespace HTTPlease.Diagnostics
{
	using MessageHandlers;
	
	/// <summary>
	/// 	Extension methods for the <see cref="ClientBuilder">HTTP client builder</see>.
	/// </summary>
	public static class ClientBuilderExtensions
	{
		/// <summary>
		/// 	Create a copy of the HTTP client builder whose clients will log requests and responses to the specified logger.
		/// </summary>
		/// <param name="logger">
		///		The logger used to log the event.
		/// </param>
		/// <returns>
		///		The new <see cref="ClientBuilder"/>.
		/// </returns>
		/// <remarks>
		///		This overload is for convenience only; for the purposes of reliability you should resolve the logger when you are creating the client (it's not good practice to share the same instance of a logger between multiple clients).
		/// </remarks>
		public static ClientBuilder WithLogging(this ClientBuilder clientBuilder, ILogger logger)
		{
			return clientBuilder.WithLogging(() => logger);
		}
		
		/// <summary>
		/// 	Create a copy of the HTTP client builder whose clients will log requests and responses to the specified logger.
		/// </summary>
		/// <param name="loggerFactory">
		///		A delegate that produces the logger for each client.
		/// </param>
		/// <returns>
		///		The new <see cref="ClientBuilder"/>.
		/// </returns>
		/// <remarks>
		///		Each call to <paramref name="loggerFactory"/> should return a new instance of the logger (it's not good practice to share the same instance of a logger between multiple clients).
		/// </remarks>
		public static ClientBuilder WithLogging(this ClientBuilder clientBuilder, Func<ILogger> loggerFactory)
		{
			return clientBuilder.AddHandler(() =>
			{
				ILogger logger = loggerFactory();
				
				return new LoggingMessageHandler(logger);
			});
		}
	}
}