using System.Net.Http;
using Xunit;

namespace HTTPlease.Formatters.Tests
{
    using Testability;

    /// <summary>
    ///    Tests for XML-formatted HTTP requests.
    /// </summary>
    public class XmlFormattedRequestTests
    {
        /// <summary>
        ///    The base request used for tests.
        /// </summary>
        static readonly HttpRequest BaseRequest = HttpRequest.Factory.Create("http://localhost/");

        /// <summary>
        ///    The base typed request used for tests.
        /// </summary>
        static readonly HttpRequest<string> TypedBaseRequest = HttpRequest<string>.Factory.Create("http://localhost/");

        /// <summary>
        ///    Verify that the ExpectXml extension method for <see cref="HttpRequest"/> adds the "application/json" XML media type to the request's Accept header.
        /// </summary>
        [Fact]
        public void Request_ExpectXml_Sets_AcceptHeader()
        {
            RequestAssert.Message(BaseRequest.ExpectXml(), HttpMethod.Get, requestMessage =>
            {
                MessageAssert.AcceptsMediaType(requestMessage, WellKnownMediaTypes.Xml);
            });
        }

        /// <summary>
        ///    Verify that the ExpectXml extension method for <see cref="HttpRequest"/> adds the "application/json" XML media type to the request's Accept header.
        /// </summary>
        [Fact]
        public void TypedRequest_ExpectXml_Sets_AcceptHeader()
        {
            RequestAssert.Message(TypedBaseRequest.ExpectXml(), HttpMethod.Get, requestMessage =>
            {
                MessageAssert.AcceptsMediaType(requestMessage, WellKnownMediaTypes.Xml);
            });
        }
    }
}
