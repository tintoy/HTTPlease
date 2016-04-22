using System.Net;
using System.Net.Http;

namespace HTTPlease
{
	/// <summary>
	///		Exception thrown when an error response is received while making an HTTP request.
	/// </summary>
	/// <remarks>
	///		TODO: Throw this from response.ReadAsAsync&lt;TResponse, TErrorResponse&gt;.
	/// </remarks>
	public class HttpRequestException<TResponse>
		: HttpRequestException
	{
		/// <summary>
		///		Create a new <see cref="HttpRequestException{TResponse}"/>.
		/// </summary>
		/// <param name="statusCode">
		///		The response's HTTP status code.
		///	</param>
		/// <param name="response">
		///		The response body.
		///	</param>
		public HttpRequestException(HttpStatusCode statusCode, TResponse response)
			: this(statusCode, response, $"The request failed with unexpected status code '{statusCode}'.")
		{
		}

		/// <summary>
		///		Create a new <see cref="HttpRequestException{TResponse}"/>.
		/// </summary>
		/// <param name="statusCode">
		///		The response's HTTP status code.
		///	</param>
		/// <param name="response">
		///		The response body.
		///	</param>
		/// <param name="message">
		///		The exception message.
		///	</param>
		public HttpRequestException(HttpStatusCode statusCode, TResponse response, string message)
			: base(message)
		{
			StatusCode = statusCode;
			Response = response;
		}

		/// <summary>
		///		The response's HTTP status code.
		/// </summary>
		public HttpStatusCode StatusCode { get; }

		/// <summary>
		///		The response body.
		/// </summary>
		public TResponse Response { get; }
	}
}
