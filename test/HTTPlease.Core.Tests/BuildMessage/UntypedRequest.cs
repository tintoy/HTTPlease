using System;
using System.Net.Http;
using Xunit;

namespace HTTPlease.Tests.BuildMessage
{
	using Testability;

	/// <summary>
	///		Message-building tests for <see cref="HttpRequest"/> (<see cref="HttpRequest.BuildRequestMessage"/> / <see cref="HttpRequest{TContext}.BuildRequestMessage"/>).
	/// </summary>
	public class UntypedRequest
	{
		/// <summary>
		///		The default context for requests used by tests.
		/// </summary>
		static readonly string DefaultContext = "Hello World";

		/// <summary>
		///		An empty request.
		/// </summary>
		static readonly HttpRequest<string> EmptyRequest = HttpRequest<string>.Empty;

		/// <summary>
		///		A request with an absolute URI.
		/// </summary>
		static readonly HttpRequest<string> AbsoluteRequest = HttpRequest<string>.Create("http://localhost:1234");

		/// <summary>
		///		A request with a relative URI.
		/// </summary>
		static readonly HttpRequest<string> RelativeRequest = HttpRequest<string>.Create("foo/bar");
		
		#region Empty requests

		/// <summary>
		///		An <see cref="HttpRequest"/> throws <see cref="InvalidOperationException"/>.
		/// </summary>
		[Fact]
		public void Empty_Throws()
		{
			Assert.Throws<InvalidOperationException>(() =>
			{
				EmptyRequest.BuildRequestMessage(HttpMethod.Get, DefaultContext);
			});
		}

		#endregion Empty requests

		#region Relative URIs

		/// <summary>
		///		An <see cref="HttpRequest"/> with a relative URI throws <see cref="InvalidOperationException"/> if no base URI is supplied.
		/// </summary>
		[Fact]
		public void RelativeUri_NoBaseUri_Throws()
		{
			Assert.Throws<InvalidOperationException>(() =>
			{
				RelativeRequest.BuildRequestMessage(HttpMethod.Get, DefaultContext);
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

			MessageAssert.FromRequest(RelativeRequest, HttpMethod.Get, DefaultContext, baseUri, requestMessage =>
			{
				Assert.NotEqual(RelativeRequest.Uri, requestMessage.RequestUri);
				Assert.Equal(expectedUri, requestMessage.RequestUri);
			});
		}

		#endregion // Relative URIs

		#region Absolute URIs

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute URI ignores the lack of a base URI and uses the request URI.
		/// </summary>
		[Fact]
		public void AbsoluteUri_NoBaseUri_UsesRequestUri()
		{
			MessageAssert.FromRequest(AbsoluteRequest, HttpMethod.Get, DefaultContext, requestMessage =>
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

			MessageAssert.FromRequest(AbsoluteRequest, HttpMethod.Get, DefaultContext, baseUri, requestMessage =>
			{
				Assert.NotEqual(baseUri, requestMessage.RequestUri);
				Assert.Equal(AbsoluteRequest.Uri, requestMessage.RequestUri);
			});
		}

		#endregion // Absolute URIs

		#region Template URIs

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute template URI, using statically-bound template parameters.
		/// </summary>
		[Fact]
		public void Absoluteuri_Template()
		{
			HttpRequest request =
				HttpRequest.Factory.Create("http://localhost:1234/{action}/{id}")
					.WithTemplateParameter("action", "foo")
					.WithTemplateParameter("id", "bar");

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal("http://localhost:1234/foo/bar",
					requestMessage.RequestUri.AbsoluteUri
				);
			});
		}

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute template URI, using dynamically-bound template parameters.
		/// </summary>
		[Fact]
		public void AbsoluteUri_Template_DeferredValues()
		{
			string action = "foo";
			string id = "bar";

			HttpRequest request =
				HttpRequest.Factory.Create("http://localhost:1234/{action}/{id}")
					.WithTemplateParameter("action", () => action)
					.WithTemplateParameter("id", () => id);

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal("http://localhost:1234/foo/bar",
					requestMessage.RequestUri.AbsoluteUri
				);
			});

			action = "diddly";
			id = "dee";

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal("http://localhost:1234/diddly/dee",
					requestMessage.RequestUri.AbsoluteUri
				);
			});
		}

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute template URI that includes a query component, using statically-bound template parameters.
		/// </summary>
		[Fact]
		public void AbsoluteUri_Template_Query()
		{
			HttpRequest request =
				HttpRequest.Factory.Create("http://localhost:1234/{action}/{id}?flag={flag}")
					.WithTemplateParameter("action", "foo")
					.WithTemplateParameter("id", "bar")
					.WithTemplateParameter("flag", true);

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal(
					"http://localhost:1234/foo/bar?flag=True",
					requestMessage.RequestUri.AbsoluteUri
				);
			});
		}

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute template URI that includes a query component, using dynamically-bound template parameters.
		/// </summary>
		[Fact]
		public void AbsoluteUri_Template_Query_DeferredValues()
		{
			string action = "foo";
			string id = "bar";
			string flag = "true";

			HttpRequest request =
				HttpRequest.Factory.Create("http://localhost:1234/")
					.WithRelativeUri("{action}/{id}?flag={flag?}")
					.WithTemplateParameter("action", () => action)
					.WithTemplateParameter("id", () => id)
					.WithTemplateParameter("flag", () => flag);

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal("http://localhost:1234/foo/bar?flag=true",
					requestMessage.RequestUri.AbsoluteUri
				);
			});

			action = "diddly";
			id = "dee";
			flag = null;

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal(
					"http://localhost:1234/diddly/dee",
					requestMessage.RequestUri.AbsoluteUri
				);
			});
		}

		#endregion // Template URIs

		#region Query parameters

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute URI that adds a query component, using statically-bound template parameters.
		/// </summary>
		[Fact]
		public void AbsoluteUri_Query()
		{
			HttpRequest request =
				HttpRequest.Factory.Create("http://localhost:1234/foo/bar")
					.WithQueryParameter("flag", true);

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal("http://localhost:1234/foo/bar?flag=True",
					requestMessage.RequestUri.AbsoluteUri
				);
			});
		}

		/// <summary>
		///		An <see cref="HttpRequest"/> with an absolute URI that adds a query component, using dynamically-bound template parameters.
		/// </summary>
		[Fact]
		public void AbsoluteUri_AddQuery_DeferredValues()
		{
			bool? flag = true;

			HttpRequest request =
				HttpRequest.Factory.Create("http://localhost:1234/foo/bar")
					.WithQueryParameter("flag", () => flag);

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal("http://localhost:1234/foo/bar?flag=True",
					requestMessage.RequestUri.AbsoluteUri
				);
			});

			flag = null;

			MessageAssert.FromRequest(request, HttpMethod.Get, requestMessage =>
			{
				Assert.Equal("http://localhost:1234/foo/bar",
					requestMessage.RequestUri.AbsoluteUri
				);
			});
		}

		#endregion // Query parameters
	}
}
