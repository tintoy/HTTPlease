using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HTTPlease.Tests
{
	using Tests.Mocks;

	/// <summary>
	///		Factory methods for <see cref="MockMessageHandler"/>s used by tests.
	/// </summary>
    public static class TestMessageHandlers
	{
		/// <summary>
		///		Create a <see cref="MockMessageHandler"/> that performs assertions on an incoming request message and returns a predefined response.
		/// </summary>
		/// <param name="assertion">
		///		A delegate that makes assertions about the incoming request message.
		/// </param>
		/// <returns>
		///		The configured <see cref="MockMessageHandler"/>.
		/// </returns>
		public static MockMessageHandler Expect(Action<HttpRequestMessage> assertion)
		{
			return Expect(HttpStatusCode.OK, assertion);
		}

		/// <summary>
		///		Create a <see cref="MockMessageHandler"/> that performs assertions on an incoming request message and returns a predefined response.
		/// </summary>
		/// <param name="statusCode">
		///		The HTTP status code for the outgoing response message.
		/// </param>
		/// <param name="assertion">
		///		A delegate that makes assertions about the incoming request message.
		/// </param>
		/// <returns>
		///		The configured <see cref="MockMessageHandler"/>.
		/// </returns>
		public static MockMessageHandler Expect(HttpStatusCode responseStatusCode, Action<HttpRequestMessage> assertion)
		{
			return new MockMessageHandler(requestMessage =>
			{
				Xunit.Assert.NotNull(requestMessage);

				assertion?.Invoke(requestMessage);

				return requestMessage.CreateResponse(responseStatusCode);
			});
		}

		/// <summary>
		///		Create a <see cref="MockMessageHandler"/> that performs assertions on an incoming request message and returns a predefined response.
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
		///		The configured <see cref="MockMessageHandler"/>.
		/// </returns>
		public static MockMessageHandler Expect(Uri expectedRequestUri, HttpMethod expectedRequestMethod, HttpStatusCode responseStatusCode, Action<HttpRequestMessage> assertion)
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
