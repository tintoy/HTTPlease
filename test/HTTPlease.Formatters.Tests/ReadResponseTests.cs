using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

using MockMessageHandler = HTTPlease.Tests.Mocks.MockMessageHandler;

namespace HTTPlease.Formatters.Tests
{
	using Json;

	/// <summary>
	///		Tests for reading responses from invoked messages.
	/// </summary>
    public class ReadResponseTests
    {
		/// <summary>
		///		The default request used for tests.
		/// </summary>
		static readonly HttpRequest DefaultRequest = HttpRequest.Create("http://localhost/");

		/// <summary>
		///		Create a new response-read test suite.
		/// </summary>
		public ReadResponseTests()
		{
		}

		/// <summary>
		///		Verify that an <see cref="HttpRequest"/>'s response can be read from
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[Fact]
		public async Task Can_Read_Success_Response_Json()
		{
			TestBody expectedBody = new TestBody
			{
				StringProperty = "This is a test",
				IntProperty = 123
			};
			MockMessageHandler mockHandler = CreateJsonMockMessageHandler(HttpStatusCode.OK, expectedBody);

			using (HttpClient client = new HttpClient(mockHandler))
			{
				TestBody actualBody = await client
					.GetAsync(DefaultRequest)
					.ReadAsAsync<TestBody>(new JsonFormatter());

				Assert.NotNull(actualBody);
				Assert.NotSame(expectedBody, actualBody);
				Assert.Equal(expectedBody.StringProperty, actualBody.StringProperty);
				Assert.Equal(expectedBody.IntProperty, actualBody.IntProperty);
			}
		}

		/// <summary>
		///		Verify that an <see cref="HttpRequest"/>'s response can be read from
		/// </summary>
		/// <returns>
		///		A <see cref="Task"/> representing asynchronous test execution.
		/// </returns>
		[Fact]
		public async Task Can_Read_Failure_Response_Json_Transformed()
		{
			TestBody expectedBody = new TestBody
			{
				StringProperty = "This is a test",
				IntProperty = 123
			};
			MockMessageHandler mockHandler = CreateJsonMockMessageHandler(HttpStatusCode.BadRequest, expectedBody);

			expectedBody.StringProperty = "This is a failure response";
			expectedBody.IntProperty = 456;

			using (HttpClient client = new HttpClient(mockHandler))
			{
				TestBody actualBody = await client
					.GetAsync(DefaultRequest)
					.ReadAsAsync(new JsonFormatter(), 
						onFailureResponse: () => new TestBody
						{
							StringProperty = expectedBody.StringProperty,
							IntProperty = expectedBody.IntProperty
						}
					);

				Assert.NotNull(actualBody);
				Assert.NotSame(expectedBody, actualBody);
				Assert.Equal(expectedBody.StringProperty, actualBody.StringProperty);
				Assert.Equal(expectedBody.IntProperty, actualBody.IntProperty);
			}
		}

		/// <summary>
		///		Create a <see cref="MockMessageHandler"/> that returns the specified status code and response body serialised as JSON.
		/// </summary>
		/// <param name="statusCode">
		///		The HTTP status code to return.
		/// </param>
		/// <param name="testBody">
		///		The <see cref="TestBody"/> that will be serialised and returned in the response body.
		/// </param>
		/// <returns>
		///		The configured <see cref="MockMessageHandler"/>.
		/// </returns>
		static MockMessageHandler CreateJsonMockMessageHandler(HttpStatusCode statusCode, TestBody testBody)
		{
			string testBodyJson = null;
			if (testBody != null)
			{
				using (StringWriter writer = new StringWriter())
				{
					new JsonSerializer().Serialize(writer, testBody);

					testBodyJson = writer.ToString();
				}
			}

			return new MockMessageHandler(
				request => CreateResponse(request, statusCode, testBodyJson)
			);
		}

		/// <summary>
		///		Create a <see cref="HttpResponseMessage"/> with the specified body.
		/// </summary>
		/// <param name="requestMessage">
		///		The request message that the response relates to.
		/// </param>
		/// <param name="statusCode">
		///		The response status code.
		/// </param>
		/// <param name="responseBody">
		///		The response body.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpResponseMessage"/>.
		/// </returns>
		static HttpResponseMessage CreateResponse(HttpRequestMessage requestMessage, HttpStatusCode statusCode, string responseBody)
		{
			if (requestMessage == null)
				throw new ArgumentNullException(nameof(requestMessage));

			HttpContent responseContent = responseBody != null ? new StringContent(responseBody) : null;

			return new HttpResponseMessage(statusCode)
			{
				Content = responseContent,
				RequestMessage = requestMessage
			};
		}

		/// <summary>
		///		A simple data-type to be deserialised from the response.
		/// </summary>
		class TestBody
		{
			/// <summary>
			///		A string property.
			/// </summary>
			public string StringProperty { get; set; }

			/// <summary>
			///		An integer property.
			/// </summary>
			public int IntProperty { get; set; }
		}
    }
}
