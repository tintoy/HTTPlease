using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HTTPlease.Testability
{
	using Mocks;

	/// <summary>
	///		Factory methods for mocked <see cref="HttpClient"/>s used by tests.
	/// </summary>
    public static class TestClients
	{
		/// <summary>
		///		Create an <see cref="HttpClient"/> that always responds to requests with the <see cref="HttpStatusCode.OK"/> status code.
		/// </summary>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient RespondWithOk()
		{
			return RespondWith(HttpStatusCode.OK);
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> that always responds to requests with the <see cref="HttpStatusCode.BadRequest"/> status code.
		/// </summary>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient RespondWithBadRequest()
		{
			return RespondWith(HttpStatusCode.BadRequest);
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> that always responds to requests with the <see cref="HttpStatusCode.BadRequest"/> status code.
		/// </summary>
		/// <param name="responseBody">
		///		A string to be used as the response message body.
		/// </param>
		/// <param name="responseMediaType">
		///		The response media type.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient RespondWithBadRequest(string responseBody, string responseMediaType)
		{
			if (String.IsNullOrWhiteSpace(responseMediaType))
				throw new ArgumentException("Must specify a valid media type.", nameof(responseMediaType));

			return RespondWith(
				request => request.CreateResponse(HttpStatusCode.BadRequest, responseBody, responseMediaType)
			);
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> that always responds to requests with the specified status code.
		/// </summary>
		/// <param name="statusCode">
		///		The HTTP status code.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient RespondWith(HttpStatusCode statusCode)
		{
			return RespondWith(
				request => request.CreateResponse(statusCode)
			);
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> that calls the specified delegate to synchronously respond to requests.
		/// </summary>
		/// <param name="handler">
		///		A delegate that takes an incoming <see cref="HttpRequest"/> and returns an outgoing <see cref="HttpResponseMessage"/>.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient RespondWith(Func<HttpRequestMessage, HttpResponseMessage> handler)
		{
			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			return new HttpClient(
				new MockMessageHandler(handler)
			);
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> that calls the specified delegate to asynchronously respond to requests.
		/// </summary>
		/// <param name="handler">
		///		A delegate that takes an incoming <see cref="HttpRequest"/> and asynchronously returns an outgoing <see cref="HttpResponseMessage"/>.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient AsyncRespondWith(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
		{
			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			return new HttpClient(
				new MockMessageHandler(handler)
			);
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> that performs assertions on an incoming request message and returns a predefined response.
		/// </summary>
		/// <param name="assertion">
		///		A delegate that makes assertions about the incoming request message.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient Expect(Action<HttpRequestMessage> assertion)
		{
			return Expect(HttpStatusCode.OK, assertion);
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> that performs assertions on an incoming request message and returns a predefined response.
		/// </summary>
		/// <param name="responseStatusCode">
		///		The HTTP status code for the outgoing response message.
		/// </param>
		/// <param name="assertion">
		///		A delegate that makes assertions about the incoming request message.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient Expect(HttpStatusCode responseStatusCode, Action<HttpRequestMessage> assertion)
		{
			return RespondWith(requestMessage =>
			{
				Assert.NotNull(requestMessage);

				assertion?.Invoke(requestMessage);

				return requestMessage.CreateResponse(responseStatusCode);
			});
		}

		/// <summary>
		///		Create an <see cref="HttpClient"/> that performs assertions on an incoming request message and returns a predefined response.
		/// </summary>
		/// <param name="expectedRequestUri">
		///		The expected URI for the incoming request message.
		/// </param>
		/// <param name="expectedRequestMethod">
		///		The expected HTTP method (e.g. GET / POST / PUT) for the incoming request message.
		/// </param>
		/// <param name="responseStatusCode">
		///		The HTTP status code for the outgoing response message.
		/// </param>
		/// <param name="assertion">
		///		A delegate that makes assertions about the incoming request message.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpClient"/>.
		/// </returns>
		public static HttpClient Expect(Uri expectedRequestUri, HttpMethod expectedRequestMethod, HttpStatusCode responseStatusCode, Action<HttpRequestMessage> assertion)
		{
			if (expectedRequestUri == null)
				throw new ArgumentNullException(nameof(expectedRequestUri));

			if (expectedRequestMethod == null)
				throw new ArgumentNullException(nameof(expectedRequestMethod));

			return Expect(responseStatusCode, requestMessage => {
				Assert.Equal(expectedRequestMethod, requestMessage.Method);
				Assert.Equal(expectedRequestUri, requestMessage.RequestUri);

				assertion?.Invoke(requestMessage);
			});
		}
	}
}
