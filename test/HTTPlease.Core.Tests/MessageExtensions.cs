using System;
using System.Net;
using System.Net.Http;

namespace HTTPlease.Tests
{
	/// <summary>
	/// 	Extension methods for <see cref="HttpRequestMessage"/> / <see cref="HttpResponseMessage"/>.
	/// </summary>
	public static class MessageExtensions
	{
		/// <summary>
		/// 	Create a <see cref="HttpResponseMessage">response message</see> with an <see cref="HttpStatusCode.OK"/> status code.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequestMessage">response message</see>.
		/// </param>
		/// <param name="statusCode">
		///		The response status code.
		/// </param>
		/// <returns>
		///		The configured response message.
		/// </returns>
		public static HttpResponseMessage CreateResponse(this HttpRequestMessage request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));
				
			return request.CreateResponse(HttpStatusCode.OK);
		}
		
		/// <summary>
		/// 	Create a <see cref="HttpResponseMessage">response message</see>.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequestMessage">response message</see>.
		/// </param>
		/// <param name="statusCode">
		///		The response status code.
		/// </param>
		/// <returns>
		///		The configured response message.
		/// </returns>
		public static HttpResponseMessage CreateResponse(this HttpRequestMessage request, HttpStatusCode statusCode)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));
			
			HttpResponseMessage response = new HttpResponseMessage(statusCode);
			try
			{
				response.RequestMessage = request;
			}
			catch
			{
				using (response)
				{
					throw;					
				}
			}
			
			return response;
		}
		
		/// <summary>
		/// 	Create a <see cref="HttpResponseMessage">response message</see>.
		/// </summary>
		/// <param name="request">
		///		The <see cref="HttpRequestMessage">response message</see>.
		/// </param>
		/// <param name="statusCode">
		///		The response status code.
		/// </param>
		/// <param name="responseBody">
		///		The response body.
		/// </param>
		/// <returns>
		///		The configured response message.
		/// </returns>
		public static HttpResponseMessage CreateResponse(this HttpRequestMessage request, HttpStatusCode statusCode, string responseBody)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));
				
			HttpResponseMessage response = request.CreateResponse(statusCode);
			if (responseBody == null)
				return response;
			
			try
			{
				response.Content = new StringContent(responseBody); 
			}
			catch
			{
				using (response)
				{
					throw;
				}
			}
			
			return response;
		}
	}
}