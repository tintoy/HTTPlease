using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPlease.Security.MessageHandlers
{
    using Abstractions;

    /// <summary>
    ///    A HTTP message handler that adds authentication to outgoing request messages.
    /// </summary>
    public sealed class AuthenticationMessageHandler
        : DelegatingHandler
    {
        /// <summary>
        ///    The authentication provider for outgoing requests.
        /// </summary>
        readonly IHttpRequestAuthenticationProvider _authenticationProvider;

        /// <summary>
        ///    Create a new <see cref="AuthenticationMessageHandler"/> that uses the specified provider for authentication.
        /// </summary>
        /// <param name="authenticationProvider">
        ///    The <see cref="IHttpRequestAuthenticationProvider">authentication provider</see> for outgoing requests.
        /// </param>
        public AuthenticationMessageHandler(IHttpRequestAuthenticationProvider authenticationProvider)
        {
            if (authenticationProvider == null)
                throw new ArgumentNullException(nameof(authenticationProvider));

            _authenticationProvider = authenticationProvider;
        }

        /// <summary>
        ///    Dispose of resources being used by the <see cref="AuthenticationMessageHandler"/>.
        /// </summary>
        /// <param name="disposing">
        ///    Explicit disposal?
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IDisposable providerDisposal = _authenticationProvider as IDisposable;
                if (providerDisposal != null)
                    providerDisposal.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        ///    Asynchronously process an HTTP request message and its response.
        /// </summary>
        /// <param name="request">
        ///    The outgoing <see cref="HttpRequestMessage"/>.
        /// </param>
        /// <param name="cancellationToken">
        ///    A <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        ///    The incoming HTTP response message.
        /// </returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
                throw new ArgumentNullException(nameof(cancellationToken));

            await _authenticationProvider.AddAuthenticationAsync(request, cancellationToken);
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
