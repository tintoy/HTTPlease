using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPlease
{
	/// <summary>
	///		Invocation-related extension methods for <see cref="HttpClient"/>s that use an <see cref="HttpRequest"/>.
	/// </summary>
	public static class UntypedClientExtensions
    {
		#region Invoke

		/// <summary>
		///		Asynchronously execute a request as an HTTP HEAD.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> HeadAsync(this HttpClient httpClient, HttpRequest request, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Head, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(requestMessage, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP GET.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, HttpRequest request, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(requestMessage, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP POST.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="postBody">
		///		Optional <see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, HttpRequest request, HttpContent postBody = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Post, postBody, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(requestMessage, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP PUT.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="putBody">
		///		<see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, HttpRequest request, HttpContent putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (putBody == null)
				throw new ArgumentNullException(nameof(putBody));

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Put, putBody, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(requestMessage, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP PATCH.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="patchBody">
		///		<see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, HttpRequest request, HttpContent patchBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (patchBody == null)
				throw new ArgumentNullException(nameof(patchBody));

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(OtherHttpMethods.Patch, patchBody, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(requestMessage, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute a request as an HTTP DELETE.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, HttpRequest request, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Delete, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(requestMessage, cancellationToken);
			}
		}

		/// <summary>
		///		Asynchronously execute the request using the specified HTTP method.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="method">
		///		An <see cref="HttpMethod"/> representing the method to use.
		/// </param>
		/// <param name="body">
		///		Optional <see cref="HttpContent"/> representing the request body (if any).
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpRequest request, HttpMethod method, HttpContent body = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			using (HttpRequestMessage requestMessage = request.BuildRequestMessage(method, body, baseUri: httpClient.BaseAddress))
			{
				return await httpClient.SendAsync(requestMessage, cancellationToken);
			}
		}

		#endregion // Invoke
	}
}
