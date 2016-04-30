using System;
using System.Net.Http;
using Xunit;

namespace HTTPlease.Tests.BuildMessage
{
	using Testability;

	/// <summary>
	///		Message-building tests for an <see cref="HttpRequest{TContext}"/>.
	/// </summary>
	public class TypedRequest
    {
		/// <summary>
		///		A request with an absolute URI.
		/// </summary>
		static readonly HttpRequest AbsoluteRequest = HttpRequest.Create("http://localhost:1234");

		/// <summary>
		///		A request with a relative URI.
		/// </summary>
		static readonly HttpRequest RelativeRequest = HttpRequest.Create("foo/bar");

		/// <summary>
		///		An <see cref="HttpRequest"/> throws <see cref="InvalidOperationException"/>.
		/// </summary>
		[Fact]
		public void Empty_Throws()
		{
			Assert.Throws<InvalidOperationException>(() =>
			{
				HttpRequest.Empty.BuildRequestMessage(HttpMethod.Get);
			});
		}

		/// <summary>
		///		An <see cref="HttpRequest"/> with a relative URI throws <see cref="InvalidOperationException"/> if no base URI is supplied.
		/// </summary>
		[Fact]
		public void RelativeUri_NoBaseUri_Throws()
		{
			Assert.Throws<InvalidOperationException>(() =>
			{
				RelativeRequest.BuildRequestMessage(HttpMethod.Get);
			});
		}

		/// <summary>
		///		An <see cref="HttpRequest"/> with a relative URI prepends the supplied base URI to the request URI.
		/// </summary>
		[Fact]
		public void RelativeUri_BaseUri_PrependsBaseUri()
		{
			Uri baseUri = new Uri("http://tintoy.io:5678/");
			Uri expectedUri = new Uri(baseUri, RelativeRequest.Uri);

			MessageAssert.FromRequest(RelativeRequest, HttpMethod.Get, baseUri, requestMessage =>
			{
				Assert.NotEqual(RelativeRequest.Uri, requestMessage.RequestUri);
				Assert.Equal(expectedUri, requestMessage.RequestUri);
			});
		}

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute URI ignores the lack of a base URI and uses the request URI.
		/// </summary>
		[Fact]
		public void AbsoluteUri_NoBaseUri_UsesRequestUri()
		{
			MessageAssert.FromRequest(AbsoluteRequest, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal(AbsoluteRequest.Uri, requestMessage.RequestUri);
			});
		}

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute URI ignores the supplied base URI and uses the request URI.
		/// </summary>
		[Fact]
		public void AbsoluteUri_BaseUri_UsesRequestUri()
		{
			Uri baseUri = new Uri("http://tintoy.io:5678/");

			MessageAssert.FromRequest(AbsoluteRequest, HttpMethod.Get, baseUri, requestMessage =>
			{
				Assert.NotEqual(baseUri, requestMessage.RequestUri);
				Assert.Equal(AbsoluteRequest.Uri, requestMessage.RequestUri);
			});
		}
	}
}
