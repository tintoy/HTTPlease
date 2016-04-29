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
		///		Delegate used to write the <see cref="LogEventIds.BeginRequest"/> event to an <see cref="ILogger"/>.
		/// </summary>
		static readonly Action<ILogger, string, Uri, Exception> _beginRequest = LoggerMessage.Define<string, Uri>(LogLevel.Verbose, LogEventIds.BeginRequest, "Performing {Method} request to '{RequestUri}.");

		/// <summary>
		///		Delegate used to write the <see cref="LogEventIds.EndRequest"/> event to an <see cref="ILogger"/>.
		/// </summary>
		static readonly Action<ILogger, string, Uri, HttpStatusCode, Exception> _endRequest = LoggerMessage.Define<string, Uri, HttpStatusCode>(LogLevel.Verbose, LogEventIds.BeginRequest, "Completed {Method} request to '{RequestUri} ({StatusCode}).");
	
		/// <summary>
		///		Delegate used to write the <see cref="LogEventIds.RequestError"/> event to an <see cref="ILogger"/>.
		/// </summary>
		static readonly Action<ILogger, string, Uri, string, Exception> _requestError = LoggerMessage.Define<string, Uri, string>(LogLevel.Error, LogEventIds.BeginRequest, "{Method} request to '{RequestUri} failed: {ErrorMessage}");

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

			_beginRequest(logger, request.Method?.Method, request.RequestUri, null);
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

			_endRequest(logger, request.Method?.Method, request.RequestUri, statusCode, null);
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

			_requestError(logger, request.Method?.Method, request.RequestUri, error.Message, error);
		}
	}
}
