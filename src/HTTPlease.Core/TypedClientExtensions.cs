using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace HTTPlease
{
	using Core;

	/// <summary>
	///		Invocation-related extension methods for <see cref="HttpClient"/>s that use an <see cref="HttpRequest{TContext}"/>.
	/// </summary>
	public static class TypedClientExtensions
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
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance used as a context for resolving deferred parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static HttpResponse HeadAsync<TContext>(this HttpClient httpClient, HttpRequest<TContext> request, TContext context, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new HttpResponse(request, async () =>
			{
				return await httpClient.SendAsync(request, HttpMethod.Head, context, cancellationToken: cancellationToken).ConfigureAwait(false);
			});
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
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance used as a context for resolving deferred parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static HttpResponse GetAsync<TContext>(this HttpClient httpClient, HttpRequest<TContext> request, TContext context, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new HttpResponse(request, async () =>
			{
				return await httpClient.SendAsync(request, HttpMethod.Get, context, cancellationToken: cancellationToken).ConfigureAwait(false);
			});
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
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance used as a context for resolving deferred parameters.
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
		public static HttpResponse PostAsync<TContext>(this HttpClient httpClient, HttpRequest<TContext> request, TContext context, HttpContent postBody = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new HttpResponse(request, async () =>
			{
				return await httpClient.SendAsync(request, HttpMethod.Post, context, postBody, cancellationToken).ConfigureAwait(false);
			});
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
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance used as a context for resolving deferred parameters.
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
		public static HttpResponse PutAsync<TContext>(this HttpClient httpClient, HttpRequest<TContext> request, TContext context, HttpContent putBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (httpClient == null)
				throw new ArgumentNullException(nameof(httpClient));

			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (putBody == null)
				throw new ArgumentNullException(nameof(putBody));

			return new HttpResponse(request, async () =>
			{
				return await httpClient.SendAsync(request, HttpMethod.Put, context, putBody, cancellationToken).ConfigureAwait(false);
			});
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
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance used as a context for resolving deferred parameters.
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
		public static HttpResponse PatchAsync<TContext>(this HttpClient httpClient, HttpRequest<TContext> request, TContext context, HttpContent patchBody, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (patchBody == null)
				throw new ArgumentNullException(nameof(patchBody));

			return new HttpResponse(request, async () =>
			{
				return await httpClient.SendAsync(request, OtherHttpMethods.Patch, context, patchBody, cancellationToken).ConfigureAwait(false);
			});
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
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance used as a context for resolving deferred parameters.
		/// </param>
		/// <param name="cancellationToken">
		///		An optional cancellation token that can be used to cancel the asynchronous operation.
		/// </param>
		/// <returns>
		///		An <see cref="HttpResponseMessage"/> representing the response.
		/// </returns>
		public static HttpResponse DeleteAsync<TContext>(this HttpClient httpClient, HttpRequest<TContext> request, TContext context, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new HttpResponse(request, async () =>
			{
				return await httpClient.SendAsync(request, HttpMethod.Delete, context, cancellationToken: cancellationToken).ConfigureAwait(false);
			});
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
		/// <param name="context">
		///		The <typeparamref name="TContext"/> instance used as a context for resolving deferred parameters.
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
		public static HttpResponse SendAsync<TContext>(this HttpClient httpClient, HttpRequest<TContext> request, HttpMethod method, TContext context, HttpContent body = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			return new HttpResponse(request, async () =>
			{
				using (HttpRequestMessage requestMessage = request.BuildRequestMessage(method, context, body, httpClient.BaseAddress))
				{
					HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
					try
					{
						request.ExecuteResponseActions(responseMessage, context);
					}
					catch
					{
						using (responseMessage)
						{
							throw;
						}
					}

					return responseMessage;
				}
			});
		}

		#endregion // Invoke

		#region Helpers

		/// <summary>
		///		Execute the request's configured response actions (if any) against the specified response message.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequest"/>.
		/// </param>
		/// <param name="responseMessage">
		///		The HTTP response message.
		/// </param>
		/// <param name="context">
		///		The <typeparamref name="TContext"/> used as a contect for resolving deferred request parameters.
		/// </param>
		static void ExecuteResponseActions<TContext>(this HttpRequest<TContext> request, HttpResponseMessage responseMessage, TContext context)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			if (responseMessage == null)
				throw new ArgumentNullException(nameof(responseMessage));

			List<Exception> responseActionExceptions = new List<Exception>();
			foreach (ResponseAction<TContext> responseAction in request.ResponseActions)
			{
				try
				{
					responseAction(responseMessage, context);
				}
				catch (Exception eResponseAction)
				{
					responseActionExceptions.Add(eResponseAction);
				}
			}

			if (responseActionExceptions.Count > 0)
				throw new AggregateException("One or more errors occurred while processing the response message.", responseActionExceptions);
		}

		#endregion // Helpers
	}
}
