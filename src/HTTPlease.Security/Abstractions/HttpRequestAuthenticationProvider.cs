using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPlease.Security.Abstractions
{
    using Core.Utilities;

    /// <summary>
    ///    The base class for HTTP request authentication mechanisms.
    /// </summary>
    public abstract class HttpRequestAuthenticationProvider
        : DisposableObject, IHttpRequestAuthenticationProvider
    {
        /// <summary>
        ///    Create new <see cref="HttpRequestAuthenticationProvider"/>.
        /// </summary>
        protected HttpRequestAuthenticationProvider()
        {
        }

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
        public abstract Task AddAuthenticationAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken = new CancellationToken());
    }
}
