using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPlease.Diagnostics.MessageHandlers
{
	/// <summary>
	///		Client-side HTTP message handler that logs outgoing requests and incoming responses.
	/// </summary>
	public class LoggingMessageHandler
		: DelegatingHandler
	{
		/// <summary>
		///		Create a new <see cref="LoggingMessageHandler"/>.
		/// </summary>
		/// <param name="logger">
		///		The <see cref="ILogger"/> used to log messages about requests and responses.
		/// </param>
		public LoggingMessageHandler(ILogger logger)
		{
			if (logger == null)
				throw new ArgumentNullException(nameof(logger));

			Log = logger;
		}

		/// <summary>
		///		The <see cref="ILogger"/> used to log messages about requests and responses.
		/// </summary>
		public ILogger Log { get; }

		/// <summary>
		///		Asynchronously process an outgoing HTTP request message and its incoming response message.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequestMessage"/> representing the outgoing request.
		/// </param>
		/// <param name="cancellationToken">
		///		A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		Create a new <see cref="LoggingMessageHandler"/>.
		/// </returns>
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			Log.BeginRequest(request);

			try
			{
				HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

				Log.EndRequest(request, response.StatusCode);

				return response;
			}
			catch (Exception eRequest)
			{
				Log.RequestError(request, eRequest);

				throw;
			}
		}
	}
}
