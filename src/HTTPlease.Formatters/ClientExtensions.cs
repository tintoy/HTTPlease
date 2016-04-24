using System;
using System.Net.Http;
using System.Threading;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		Extension methods for invocation of untyped <see cref="HttpRequest"/>s using an <see cref="HttpClient"/>.
	/// </summary>
	public static class ClientExtensions
	{
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
		///		An optional object to be used as the the request body.
		/// </param>
		/// <param name="mediaType">
		///		If <paramref name="postBody"/> is specified, the media type to be used
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponse"/> representing the response.
		/// </returns>
		public static HttpResponse PostAsync(this HttpClient httpClient, HttpRequest request, object postBody, string mediaType, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new HttpResponse(request, async () =>
			{
				using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Post, postBody, mediaType, baseUri: httpClient.BaseAddress))
				{
					return await httpClient.SendAsync(requestMessage, cancellationToken);
				}
			});
		}

		/// <summary>
		///		Asynchronously perform an HTTP POST request, serialising the request to JSON.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="postBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponse"/> representing the response.
		/// </returns>
		public static HttpResponse PostAsJsonAsync(this HttpClient httpClient, HttpRequest request, object postBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PostAsync(request, postBody, WellKnownMediaTypes.Json, cancellationToken);
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
		///		An optional object to be used as the the request body.
		/// </param>
		/// <param name="mediaType">
		///		If <paramref name="putBody"/> is specified, the media type to be used
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponse"/> representing the response.
		/// </returns>
		public static HttpResponse PutAsync(this HttpClient httpClient, HttpRequest request, object putBody, string mediaType, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new HttpResponse(request, async () =>
			{
				using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Put, putBody, mediaType, baseUri: httpClient.BaseAddress))
				{
					return await httpClient.SendAsync(requestMessage, cancellationToken);
				}
			});
		}

		/// <summary>
		///		Asynchronously perform an HTTP PUT request, serialising the request to JSON.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="putBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponse"/> representing the response.
		/// </returns>
		public static HttpResponse PutAsJsonAsync(this HttpClient httpClient, HttpRequest request, object putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PutAsync(request, putBody, WellKnownMediaTypes.Json, cancellationToken);
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
		///		An optional object to be used as the the request body.
		/// </param>
		/// <param name="mediaType">
		///		If <paramref name="patchBody"/> is specified, the media type to be used
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponse"/> representing the response.
		/// </returns>
		public static HttpResponse PatchAsync(this HttpClient httpClient, HttpRequest request, object patchBody, string mediaType, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new HttpResponse(request, async () =>
			{
				using (HttpRequestMessage requestMessage = request.BuildRequestMessage(OtherHttpMethods.Patch, patchBody, mediaType, baseUri: httpClient.BaseAddress))
				{
					return await httpClient.SendAsync(requestMessage, cancellationToken);
				}
			});
		}

		/// <summary>
		///		Asynchronously perform an HTTP PATCH request, serialising the request to JSON.
		/// </summary>
		/// <param name="httpClient">
		///		The <see cref="HttpClient"/> used to execute the request.
		/// </param>
		/// <param name="request">
		///		The HTTP request.
		/// </param>
		/// <param name="patchBody">
		///		The object that will be serialised into the request body.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponse"/> representing the response.
		/// </returns>
		public static HttpResponse PatchAsJsonAsync(this HttpClient httpClient, HttpRequest request, object patchBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			return httpClient.PatchAsync(request, patchBody, WellKnownMediaTypes.Json, cancellationToken);
		}
	}
}
