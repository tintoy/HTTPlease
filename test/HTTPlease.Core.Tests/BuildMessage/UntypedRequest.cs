using System;
using System.Net.Http;
using Xunit;

namespace HTTPlease.Tests.BuildMessage
{
    using Testability;
    using Xunit.Abstractions;

    /// <summary>
    ///    Message-building tests for <see cref="HttpRequest"/> (<see cref="HttpRequest.BuildRequestMessage"/>).
    /// </summary>
    public class UntypedRequest
    {
        /// <summary>
        ///    A request with an absolute URI.
        /// </summary>
        static readonly HttpRequest AbsoluteRequest = HttpRequest.Create("http://localhost:1234");

        /// <summary>
        ///    A request with a relative URI.
        /// </summary>
        static readonly HttpRequest RelativeRequest = HttpRequest.Create("foo/bar");

        public UntypedRequest(ITestOutputHelper testOutput)
        {
            if (testOutput == null)
                throw new ArgumentNullException(nameof(testOutput));

            TestOutput = testOutput;
        }

        ITestOutputHelper TestOutput { get; }

        /// <summary>
        ///    An <see cref="HttpRequest"/> throws <see cref="InvalidOperationException"/>.
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
        ///    An <see cref="HttpRequest"/> with a relative URI throws <see cref="InvalidOperationException"/> if no base URI is supplied.
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
        ///    An <see cref="HttpRequest"/> with a relative URI prepends the supplied base URI to the request URI.
        /// </summary>
        [Fact]
        public void RelativeUri_BaseUri_PrependsBaseUri()
        {
            Uri baseUri = new Uri("http://tintoy.io:5678/");

            RequestAssert.MessageHasUri(RelativeRequest, baseUri,
                expectedUri: new Uri(baseUri, RelativeRequest.Uri)
            );
        }

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute URI ignores the lack of a base URI and uses the request URI.
        /// </summary>
        [Fact]
        public void AbsoluteUri_NoBaseUri_UsesRequestUri()
        {
            RequestAssert.MessageHasUri(AbsoluteRequest,
                expectedUri: AbsoluteRequest.Uri
            );
        }

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute URI ignores the supplied base URI and uses the request URI.
        /// </summary>
        [Fact]
        public void AbsoluteUri_BaseUri_UsesRequestUri()
        {
            Uri baseUri = new Uri("http://tintoy.io:5678/");

            RequestAssert.MessageHasUri(AbsoluteRequest, baseUri,
                expectedUri: AbsoluteRequest.Uri
            );
        }

        #region Template URIs

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute template URI, using statically-bound template parameters.
        /// </summary>
        [Fact]
        public void Absoluteuri_Template()
        {
            HttpRequest request =
                HttpRequest.Factory.Create("http://localhost:1234/{action}/{id}")
                    .WithTemplateParameter("action", "foo")
                    .WithTemplateParameter("id", "bar");

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar"
            );
        }

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute template URI, using dynamically-bound template parameters.
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

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar"
            );
            
            action = "diddly";
            id = "dee";

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/diddly/dee"
            );
        }

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute template URI that includes a query component, using statically-bound template parameters.
        /// </summary>
        [Fact]
        public void AbsoluteUri_Template_Query()
        {
            HttpRequest request =
                HttpRequest.Factory.Create("http://localhost:1234/{action}/{id}?value={value}")
                    .WithTemplateParameter("action", "foo")
                    .WithTemplateParameter("id", "bar")
                    .WithTemplateParameter("value", true);

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?value=True"
            );
        }

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute template URI that includes a query component, using dynamically-bound template parameters.
        /// </summary>
        [Fact]
        public void AbsoluteUri_Template_Query_DeferredValues()
        {
            string action = "foo";
            string id = "bar";
            bool? value = true;

            HttpRequest request =
                HttpRequest.Factory.Create("http://localhost:1234/")
                    .WithRelativeUri("{action}/{id}?value={value?}")
                    .WithTemplateParameter("action", () => action)
                    .WithTemplateParameter("id", () => id)
                    .WithTemplateParameter("value", () => value);

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?value=True"
            );

            action = "diddly";
            id = "dee";
            value = null;

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/diddly/dee"
            );
        }

        #endregion // Template URIs

        #region Query parameters

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute URI that adds a query component, using statically-bound template parameters.
        /// </summary>
        [Fact]
        public void AbsoluteUri_Query()
        {
            HttpRequest request =
                HttpRequest.Factory.Create("http://localhost:1234/foo/bar")
                    .WithQueryParameter("value", true);

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?value=True"
            );
        }

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute URI that adds a query component, using dynamically-bound template parameters.
        /// </summary>
        [Fact]
        public void AbsoluteUri_AddQuery_DeferredValues()
        {
            bool? value = true;

            HttpRequest request =
                HttpRequest.Factory.Create("http://localhost:1234/foo/bar")
                    .WithQueryParameter("value", () => value);

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?value=True"
            );
            
            value = null;

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar"
            );
        }

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute URI that adds a multiple (duplicate) parameter query component, using dynamically-bound template parameters.
        /// </summary>
        [Fact]
        public void AbsoluteUri_AddQuery_Multiple_DeferredValues()
        {
            bool? value1 = true;
            bool? value2 = false;

            HttpRequest request =
                HttpRequest.Factory.Create("http://localhost:1234/foo/bar")
                    .WithQueryParameter("value", () => value1)
                    .WithQueryParameter("value", () => value2);

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?value=True&value=False"
            );

            value1 = null;

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?value=False"
            );

            value2 = null;

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar"
            );
        }

        /// <summary>
        ///    An <see cref="HttpRequest"/> with an absolute URI that adds a multiple (duplicate) flag parameter query component, using dynamically-bound template parameters.
        /// </summary>
        [Fact]
        public void AbsoluteUri_AddQuery_Flag_Multiple_DeferredValues()
        {
            string value1 = String.Empty;
            string value2 = String.Empty;

            HttpRequest request =
                HttpRequest.Factory.Create("http://localhost:1234/foo/bar")
                    .WithQueryParameter("flag", () => value1)
                    .WithQueryParameter("flag", () => value2);

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?flag&flag"
            );

            value1 = null;

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?flag"
            );

            value2 = null;

            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar"
            );
        }

        /// <summary>
        ///     An <see cref="HttpRequest"/> with an absolute URI that adds a query component, with no additional path component).
        /// </summary>
        [Fact]
        public void AbsoluteUri_AddQuery_EmptyPath()
        {
            HttpRequest request =
                AbsoluteRequest.WithRelativeUri("foo/bar")
                    .WithRelativeUri("?baz=bonk");
            
            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?baz=bonk"
            );
        }

        /// <summary>
        ///     An <see cref="HttpRequest"/> with an absolute URI that adds a query component, with no additional path component).
        /// </summary>
        [Fact]
        public void AbsoluteUri_WithQuery_AddQuery_EmptyPath()
        {
            HttpRequest request =
                AbsoluteRequest.WithRelativeUri("foo/bar?baz=bonk")
                    .WithRelativeUri("?bo=diddly");
            
            RequestAssert.MessageHasUri(request,
                expectedUri: "http://localhost:1234/foo/bar?baz=bonk&bo=diddly"
            );
        }

        #endregion // Query parameters
    }
}
