using System.Net.Http;
using Xunit;

namespace HTTPlease.Formatters.Tests
{
	using HTTPlease.Tests;
	
	/// <summary>
	///		Tests for JSON-formatted HTTP requests.
	/// </summary>
    public class JsonFormattedRequestTests
    {
		/// <summary>
		///		The base request used for tests.
		/// </summary>
		static readonly HttpRequest BaseRequest = HttpRequest.Factory.Create("http://localhost/");

		/// <summary>
		///		The base typed request used for tests.
		/// </summary>
		static readonly HttpRequest<string> TypedBaseRequest = HttpRequest<string>.Factory.Create("http://localhost/");

		/// <summary>
		///		Verify that the ExpectJson extension method for <see cref="HttpRequest"/> adds the "application/json" JSON media type to the request's Accept header.
		/// </summary>
		[Fact]
		public void Request_ExpectJson_Sets_AcceptHeader()
		{
			MessageAssert.Request(BaseRequest.ExpectJson(), HttpMethod.Get, requestMessage =>
			{
				MessageAssert.AcceptsMediaType(requestMessage, WellKnownMediaTypes.Json);
			});
		}

		/// <summary>
		///		Verify that the ExpectJson extension method for <see cref="HttpRequest"/> adds the "application/json" JSON media type to the request's Accept header.
		/// </summary>
		[Fact]
		public void TypedRequest_ExpectJson_Sets_AcceptHeader()
		{
			MessageAssert.Request(TypedBaseRequest.ExpectJson(), HttpMethod.Get, requestMessage =>
			{
				MessageAssert.AcceptsMediaType(requestMessage, WellKnownMediaTypes.Json);
			});
		}
	}
}
