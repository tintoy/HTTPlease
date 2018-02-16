using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPlease.Security.Abstractions
{
	/// <summary>
	///		Represents a mechanism for adding authentication information to HTTP request messages.
	/// </summary>
	public interface IHttpRequestAuthenticationProvider
	{
		/// <summary>
		///		Asynchronously configure an outgoing request message to add information required for authentication.
		/// </summary>
		/// <param name="requestMessage">
		///		The outgoing HTTP request message.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional <see cref="CancellationToken"/> that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		A <see cref="Task"/> representing the asynchronous operation.
		/// </returns>
		Task AddAuthenticationAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken = default);
	}
}
