using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;

namespace HTTPlease.Diagnostics.MessageHandlers
{
	/// <summary>
	///		Extension methods for <see cref="ILogger"/> used to log messages about requests and responses.
	/// </summary>
	public static class LoggerExtensions
	{
		/// <summary>
		///		The Ids of well-known log events raised by HTTPlease diagnostics.
		/// </summary>
		public static class LogEventIds
		{
			/// <summary>
			/// 	An outgoing HTTP request is being performed. 
			/// </summary>
			public static readonly int BeginRequest	= 100;
			
			/// <summary>
			/// 	An incoming HTTP response has been received.
			/// </summary>
			public static readonly int EndRequest	= 101;
			
			/// <summary>
			/// 	An exception occurred while performing an HTTP request.
			/// </summary>
			public static readonly int RequestError	= 102;
		}

		/// <summary>
		/// 	Log an event representing the start of an HTTP request.
		/// </summary>
		/// <param name="logger">
		///		The logger used to log the event.
		/// </param>
		/// <param name="request">
		///		An <see cref="HttpRequestMessage"/> representing the request.
		/// </param>
		public static void BeginRequest(this ILogger logger, HttpRequestMessage request)
		{
			if (logger == null)
				throw new ArgumentNullException(nameof(logger));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			logger.LogDebug(LogEventIds.BeginRequest, "Performing {Method} request to '{RequestUri}.",
				request.Method?.Method,
				request.RequestUri
			);
		}

		
		/// <summary>
		/// 	Log an event representing the completion of an HTTP request.
		/// </summary>
		/// <param name="logger">
		///		The logger used to log the event.
		/// </param>
		/// <param name="request">
		///		An <see cref="HttpRequestMessage"/> representing the request.
		/// </param>
		/// <param name="statusCode">
		///		An <see cref="HttpStatusCode"/> representing the response status code.
		/// </param>
		public static void EndRequest(this ILogger logger, HttpRequestMessage request, HttpStatusCode statusCode)
		{
			if (logger == null)
				throw new ArgumentNullException(nameof(logger));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			logger.LogDebug(LogEventIds.EndRequest, "Completed {Method} request to '{RequestUri} ({StatusCode}).",
				request.Method?.Method,
				request.RequestUri,
				statusCode
			);
		}

		/// <summary>
		/// 	Log an event representing an error encountered while performing an HTTP request.
		/// </summary>
		/// <param name="logger">
		///		The logger used to log the event.
		/// </param>
		/// <param name="request">
		///		An <see cref="HttpRequestMessage"/> representing the request.
		/// </param>
		/// <param name="error">
		///		An <see cref="Exception"/> representing the error.
		/// </param>
		public static void RequestError(this ILogger logger, HttpRequestMessage request, Exception error)
		{
			if (logger == null)
				throw new ArgumentNullException(nameof(logger));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (error == null)
				throw new ArgumentNullException(nameof(error));

			logger.LogDebug(LogEventIds.RequestError, error, "{Method} request to '{RequestUri} failed: {ErrorMessage}",
				request.Method?.Method,
				request.RequestUri,
				error.Message
			);
		}
	}
}
