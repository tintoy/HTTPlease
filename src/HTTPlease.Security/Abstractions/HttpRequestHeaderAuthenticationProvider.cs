using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPlease.Security.Abstractions
{
    /// <summary>
    ///    The base class for authentication using the HTTP "Authorization" header.
    /// </summary>
    public abstract class HttpRequestHeaderAuthenticationProvider
        : HttpRequestAuthenticationProvider
    {
        /// <summary>
        ///    Create new <see cref="HttpRequestAuthenticationProvider"/>.
        /// </summary>
        protected HttpRequestHeaderAuthenticationProvider()
        {
        }

        /// <summary>
        ///    The name of the authentication scheme to add to the "Authorization" header.
        /// </summary>
        public abstract string AuthenticationScheme { get; }

        /// <summary>
        ///    Asynchronously provide an "Authorization" header value for the specified request URI.
        /// </summary>
        /// <param name="requestUri">
        ///    The HTTP request URI to which the header will apply.
        /// </param>
        /// <param name="cancellationToken">
        ///    An optional <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        ///    The authentication parameter value (will be appended to the <see cref="AuthenticationScheme"/>).
        /// </returns>
        public abstract Task<string> GetAuthenticationParameterAsync(Uri requestUri, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///    Asynchronously configure an outgoing request message to add information required for authentication.
        /// </summary>
        /// <param name="requestMessage">
        ///    The outgoing HTTP request message.
        /// </param>
        /// <param name="cancellationToken">
        ///    An optional <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        ///    A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        public sealed override async Task AddAuthenticationAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken = new CancellationToken())
        {
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));

            string authenticationParameter = await GetAuthenticationParameterAsync(requestMessage.RequestUri, cancellationToken);
            if (authenticationParameter == null)
                return;

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(AuthenticationScheme, authenticationParameter);
        }
    }
}
