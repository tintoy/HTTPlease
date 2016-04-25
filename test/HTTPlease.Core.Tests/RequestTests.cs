using System;
using System.Net.Http;
using Xunit;

namespace HTTPlease.Tests
{
	/// <summary>
	///		Unit-tests for <see cref="HttpRequest"/>.
	/// </summary>
	public sealed class RequestTests
	{
		/// <summary>
		///		Verify that a request builder can build a message with an absolute and then relative template URI.
		/// </summary>
		[Fact]
		public void Request_RelativeTemplateUri_BuildMessage()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			HttpRequest request =
				HttpRequest.Create.FromUri(baseUri)
					.WithRelativeRequestUri("{action}/{id}")
					.WithTemplateParameter("action", "foo")
					.WithTemplateParameter("id", "bar");
			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "foo/bar"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a message with an absolute and then relative template URI with deferred values.
		/// </summary>
		[Fact]
		public void Request_RelativeTemplateUri_DeferredValues_BuildMessage()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			string action = "foo";
			string id = "bar";

			HttpRequest request =
				HttpRequest.Create.FromUri(baseUri)
					.WithRelativeRequestUri("{action}/{id}")
					.WithTemplateParameter("action", () => action)
					.WithTemplateParameter("id", () => id);

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "foo/bar"),
					requestMessage.RequestUri
				);
			}

			action = "diddly";
			id = "dee";

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "diddly/dee"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a message with an absolute and then relative template URI (with query parameters) .
		/// </summary>
		[Fact]
		public void Request_RelativeTemplateUriWithQuery_BuildMessage()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			HttpRequest request =
				HttpRequest.Create.FromUri(baseUri)
					.WithRelativeRequestUri("{action}/{id}?flag={flag}")
					.WithTemplateParameter("action", "foo")
					.WithTemplateParameter("id", "bar")
					.WithTemplateParameter("flag", "true");
			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "foo/bar?flag=true"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative template URI (with query parameters) with deferred values.
		/// </summary>
		[Fact]
		public void Request_RelativeTemplateUriWithQuery_DeferredValues_Build()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			string action = "foo";
			string id = "bar";
			string flag = "true";

			HttpRequest request =
				HttpRequest.Create.FromUri(baseUri)
					.WithRelativeRequestUri("{action}/{id}?flag={flag?}")
					.WithTemplateParameter("action", () => action)
					.WithTemplateParameter("id", () => id)
					.WithTemplateParameter("flag", () => flag);

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "foo/bar?flag=true"),
					requestMessage.RequestUri
				);
			}

			action = "diddly";
			id = "dee";
			flag = null;

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "diddly/dee"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative URI with query parameters.
		/// </summary>
		[Fact]
		public void Request_RelativeUriWithQuery_Build()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			HttpRequest request =
				HttpRequest.Create.FromUri(baseUri)
					.WithRelativeRequestUri("foo/bar")
					.WithQueryParameter("flag", "true");
			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "foo/bar?flag=true"),
					requestMessage.RequestUri
				);
			}
		}

		/// <summary>
		///		Verify that a request builder can build a request with an absolute and then relative URI with query parameters that have deferred values.
		/// </summary>
		[Fact]
		public void Request_RelativeUriWithQuery_DeferredValues_Build()
		{
			Uri baseUri = new Uri("http://localhost:1234/");

			string flag = "true";

			HttpRequest request =
				HttpRequest.Create.FromUri(baseUri)
					.WithRelativeRequestUri("foo/bar")
					.WithQueryParameter("flag", () => flag);

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "foo/bar?flag=true"),
					requestMessage.RequestUri
				);
			}

			flag = null;

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
			{
				Assert.Equal(
					new Uri(baseUri, "foo/bar"),
					requestMessage.RequestUri
				);
			}
		}
	}
}
