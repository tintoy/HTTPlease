using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTPlease.Formatters
{
	/// <summary>
	///		Extension methods for the <see cref="HttpResponseMessage"/>s returned asynchronously by invocation of <see cref="HttpRequest"/>s by <see cref="HttpClient"/>s.
	/// </summary>
	public static class ResponseExtensions
    {
		/// <summary>
		///		Asynchronously read the response body as the specified type using a specific content formatter.
		/// </summary>
		/// <typeparam name="TBody">
		///		The CLR type into which the body content will be deserialised.
		/// </typeparam>
		/// <param name="response">
		///		The asynchronous response.
		/// </param>
		/// <param name="formatter">
		///		A <see cref="IInputFormatter"/> that will be used to read the response body.
		/// </param>
		/// <param name="expectedStatusCodes">
		///		Optional <see cref="HttpStatusCode"/>s that are expected and should therefore not prevent the response from being deserialised.
		/// 
		///		If not specified, then the standard behaviour provided by <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/> is used.
		/// </param>
		/// <returns>
		///		The deserialised response body.
		/// </returns>
		public static async Task<TBody> ReadAsAsync<TBody>(this Task<HttpResponseMessage> response, IInputFormatter formatter, params HttpStatusCode[] expectedStatusCodes)
		{
			if (response == null)
				throw new ArgumentNullException(nameof(response));

			if (formatter == null)
				throw new ArgumentNullException(nameof(formatter));

			using (HttpResponseMessage responseMessage = await response.ConfigureAwait(false))
			{
				if (!expectedStatusCodes.Contains(responseMessage.StatusCode))
					responseMessage.EnsureSuccessStatusCode(); // Default behaviour.

				if (!responseMessage.HasBody())
					throw new InvalidOperationException("The response body is empty."); // TODO: Custom exception type.

				return await responseMessage.Content.ReadAsAsync<TBody>(formatter).ConfigureAwait(false);
			}
		}

		/// <summary>
		///		Asynchronously read the response body as the specified type using a specific content formatter.
		/// </summary>
		/// <typeparam name="TBody">
		///		The CLR type into which the body content will be deserialised.
		/// </typeparam>
		/// <param name="response">
		///		The asynchronous response.
		/// </param>
		/// <param name="formatters">
		///		The <see cref="IFormatterCollection"/> that will be used to select an appropriate content formatter for reading the response body.
		/// </param>
		/// <param name="expectedStatusCodes">
		///		Optional <see cref="HttpStatusCode"/>s that are expected and should therefore not prevent the response from being deserialised.
		/// 
		///		If not specified, then the standard behaviour provided by <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/> is used.
		/// </param>
		/// <returns>
		///		The deserialised response body.
		/// </returns>
		public static async Task<TBody> ReadAsAsync<TBody>(this Task<HttpResponseMessage> response, IFormatterCollection formatters, params HttpStatusCode[] expectedStatusCodes)
		{
			if (response == null)
				throw new ArgumentNullException(nameof(response));

			if (formatters == null)
				throw new ArgumentNullException(nameof(formatters));

			using (HttpResponseMessage responseMessage = await response.ConfigureAwait(false))
			{
				if (!expectedStatusCodes.Contains(responseMessage.StatusCode))
					responseMessage.EnsureSuccessStatusCode(); // Default behaviour.

				return await responseMessage.ReadContentAsAsync<TBody>(formatters).ConfigureAwait(false);
			}
		}

		/// <summary>
		///		Asynchronously read the response body as the specified type using a specific content formatter.
		/// </summary>
		/// <typeparam name="TBody">
		///		The CLR type into which the body content will be deserialised.
		/// </typeparam>
		/// <param name="response">
		///		The asynchronous response.
		/// </param>
		/// <param name="expectedStatusCodes">
		///		Optional <see cref="HttpStatusCode"/>s that are expected and should therefore not prevent the response from being deserialised.
		/// 
		///		If not specified, then the standard behaviour provided by <see cref="HttpResponseMessage.EnsureSuccessStatusCode"/> is used.
		/// </param>
		/// <returns>
		///		The deserialised response body.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		No content formatters were configured for the request that generated the response message.
		///		
		///		Consider using the overload of ReadAsAsync that takes a specific <see cref="IInputFormatter"/>.
		/// </exception>
		public static async Task<TBody> ReadAsAsync<TBody>(this Task<HttpResponseMessage> response, params HttpStatusCode[] expectedStatusCodes)
		{
			if (response == null)
				throw new ArgumentNullException(nameof(response));

			using (HttpResponseMessage responseMessage = await response.ConfigureAwait(false))
			{
				if (!expectedStatusCodes.Contains(responseMessage.StatusCode))
					responseMessage.EnsureSuccessStatusCode(); // Default behaviour.

				return await responseMessage.ReadContentAsAsync<TBody>().ConfigureAwait(false);
			}
		}

		/// <summary>
		///		Asynchronously read the response body as the specified type.
		/// </summary>
		/// <typeparam name="TBody">
		///		The CLR type into which the body content will be deserialised.
		/// </typeparam>
		/// <param name="response">
		///		The asynchronous response.
		/// </param>
		/// <param name="formatter">
		///		The <see cref="IInputFormatter"/> that will be used to read the response body.
		/// </param>
		/// <param name="onFailureResponse">
		///		A delegate that is called to get a <typeparamref name="TBody"/> in the event that the response status code is not valid.
		/// </param>
		/// <param name="successStatusCodes">
		///		Optional <see cref="HttpStatusCode"/>s that should be treated as representing a successful response.
		/// </param>
		/// <returns>
		///		The deserialised body.
		/// </returns>
		public static Task<TBody> ReadAsAsync<TBody>(this Task<HttpResponseMessage> response, IInputFormatter formatter, Func<TBody> onFailureResponse, params HttpStatusCode[] successStatusCodes)
		{
			if (response == null)
				throw new ArgumentNullException(nameof(response));

			if (formatter == null)
				throw new ArgumentNullException(nameof(formatter));

			if (onFailureResponse == null)
				throw new ArgumentNullException(nameof(onFailureResponse));

			return response.ReadAsAsync(formatter, responseMessage => onFailureResponse(), successStatusCodes);
		}

		/// <summary>
		///		Asynchronously read the response body as the specified type.
		/// </summary>
		/// <typeparam name="TBody">
		///		The CLR type into which the body content will be deserialised.
		/// </typeparam>
		/// <param name="response">
		///		The asynchronous response.
		/// </param>
		/// <param name="formatter">
		///		The <see cref="IInputFormatter"/> that will be used to read the response body.
		/// </param>
		/// <param name="onFailureResponse">
		///		A delegate that is called to get a <typeparamref name="TBody"/> in the event that the response status code is not valid.
		/// </param>
		/// <param name="successStatusCodes">
		///		Optional <see cref="HttpStatusCode"/>s that should be treated as representing a successful response.
		/// </param>
		/// <returns>
		///		The deserialised body.
		/// </returns>
		public static async Task<TBody> ReadAsAsync<TBody>(this Task<HttpResponseMessage> response, IInputFormatter formatter, Func<HttpResponseMessage, TBody> onFailureResponse, params HttpStatusCode[] successStatusCodes)
		{
			if (response == null)
				throw new ArgumentNullException(nameof(response));

			if (onFailureResponse == null)
				throw new ArgumentNullException(nameof(onFailureResponse));

			using (HttpResponseMessage responseMessage = await response.ConfigureAwait(false))
			{
				if (!successStatusCodes.Contains(responseMessage.StatusCode) && !responseMessage.IsSuccessStatusCode)
					return onFailureResponse(responseMessage);

				responseMessage.EnsureHasBody();

				return await responseMessage.ReadContentAsAsync<TBody>().ConfigureAwait(false);
			}
		}

		/// <summary>
		///		Determine if the response has body content.
		/// </summary>
		/// <param name="responseMessage">
		///		The response message.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the response has a non-zero content length.
		/// </returns>
		public static bool HasBody(this HttpResponseMessage responseMessage)
		{
			if (responseMessage == null)
				throw new ArgumentNullException(nameof(responseMessage));

			if (responseMessage.Content == null)
				return false;

			return responseMessage.Content.Headers.ContentLength.GetValueOrDefault() > 0;
		}

		/// <summary>
		///		Ensure that the response has body content.
		/// </summary>
		/// <param name="responseMessage">
		///		The response message.
		/// </param>
		/// <returns>
		///		The response message (enables inline use).
		/// </returns>
		public static HttpResponseMessage EnsureHasBody(this HttpResponseMessage responseMessage)
		{
			if (responseMessage == null)
				throw new ArgumentNullException(nameof(responseMessage));

			if (responseMessage.HasBody())
				return responseMessage;

			throw new InvalidOperationException("The response body is empty."); // TODO: Consider custom exception type.
		}

		/// <summary>
		///		Deserialise the response message's content into the specified CLR data type using the most appropriate formatter.
		/// </summary>
		/// <typeparam name="TBody">
		///		The CLR data type into which the body will be deserialised.
		/// </typeparam>
		/// <param name="responseMessage">
		///		The response message.
		/// </param>
		/// <returns>
		///		The deserialised message body.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		No formatters were configured for the request, or an appropriate formatter could not be found in the request's list of formatters.
		/// </exception>
		public static async Task<TBody> ReadContentAsAsync<TBody>(this HttpResponseMessage responseMessage)
		{
			if (responseMessage == null)
				throw new ArgumentNullException(nameof(responseMessage));

			responseMessage.EnsureHasBody();

			IFormatterCollection formatters = responseMessage.GetFormatters();
			if (formatters == null)
				throw new InvalidOperationException("No content formatters were configured for the request that generated the response message."); // TODO: Consider custom exception type.

			return await responseMessage.ReadContentAsAsync<TBody>(formatters).ConfigureAwait(false);
		}

		/// <summary>
		///		Deserialise the response message's content into the specified CLR data type using the most appropriate formatter.
		/// </summary>
		/// <typeparam name="TBody">
		///		The CLR data type into which the body will be deserialised.
		/// </typeparam>
		/// <param name="responseMessage">
		///		The response message.
		/// </param>
		/// <param name="formatters">
		///		The collection of content formatters from which to select an appropriate formatter.
		/// </param>
		/// <returns>
		///		The deserialised message body.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		An appropriate formatter could not be found in the request's list of formatters.
		/// </exception>
		public static async Task<TBody> ReadContentAsAsync<TBody>(this HttpResponseMessage responseMessage, IFormatterCollection formatters)
		{
			if (responseMessage == null)
				throw new ArgumentNullException(nameof(responseMessage));

			if (formatters == null)
				throw new ArgumentNullException(nameof(formatters));

			responseMessage.EnsureHasBody();

			InputFormatterContext readContext = responseMessage.Content.CreateInputFormatterContext<TBody>();
			IInputFormatter readFormatter = formatters.FindInputFormatter(readContext);
			if (readFormatter == null)
				throw new InvalidOperationException($"None of the supplied formatters can read content of type '{readContext.ContentType}' into CLR type '{readContext.DataType.FullName}'.");

			return await responseMessage.ReadContentAsAsync<TBody>(readFormatter, readContext).ConfigureAwait(false);
		}

		/// <summary>
		///		Deserialise the response message's body content into the specified CLR data type using the most appropriate formatter.
		/// </summary>
		/// <typeparam name="TBody">
		///		The CLR data type into which the body content will be deserialised.
		/// </typeparam>
		/// <param name="responseMessage">
		///		The response message.
		/// </param>
		/// <param name="formatter">
		///		The content formatter that will be used to deserialise the body content.
		/// </param>
		/// <param name="formatterContext">
		///		Contextual information for the formatter about the body content.
		/// </param>
		/// <returns>
		///		The deserialised message body.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		An appropriate formatter could not be found in the request's list of formatters.
		/// </exception>
		public static async Task<TBody> ReadContentAsAsync<TBody>(this HttpResponseMessage responseMessage, IInputFormatter formatter, InputFormatterContext formatterContext)
		{
			if (responseMessage == null)
				throw new ArgumentNullException(nameof(responseMessage));

			if (formatter == null)
				throw new ArgumentNullException(nameof(formatter));

			if (formatterContext == null)
				throw new ArgumentNullException(nameof(formatterContext));

			responseMessage.EnsureHasBody();

			return await responseMessage.Content.ReadAsAsync<TBody>(formatter, formatterContext).ConfigureAwait(false);
		}
	}
}
