using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace HTTPlease
{
	using Requests;
	using Utilities;
	using ValueProviders;

	using RequestProperties = ImmutableDictionary<string, object>;

	/// <summary>
	///		A template for an HTTP request.
	/// </summary>
	public sealed class HttpRequest
		: HttpRequestBase, IUntypedHttpRequest
	{
		#region Constants

		/// <summary>
		///		The base properties for <see cref="HttpRequest"/>s.
		/// </summary>
		static readonly RequestProperties BaseProperties =
			new Dictionary<string, object>
			{
				[nameof(RequestActions)] = ImmutableList<RequestAction>.Empty,
				[nameof(TemplateParameters)] = ImmutableDictionary<string, ISimpleValueProvider<string>>.Empty,
				[nameof(QueryParameters)] = ImmutableDictionary<string, ISimpleValueProvider<string>>.Empty
			}
			.ToImmutableDictionary();

		#endregion // Constants

		#region Construction

		/// <summary>
		///		Create a new HTTP request.
		/// </summary>
		/// <param name="properties">
		///		The request properties.
		/// </param>
		HttpRequest(ImmutableDictionary<string, object> properties)
			: base(properties)
		{
			EnsurePropertyType<ImmutableList<RequestAction>>(
				propertyName: nameof(RequestActions)
			);
			EnsurePropertyType<ImmutableDictionary<string, ISimpleValueProvider<string>>>(
				propertyName: nameof(TemplateParameters)
			);
			EnsurePropertyType<ImmutableDictionary<string, ISimpleValueProvider<string>>>(
				propertyName: nameof(QueryParameters)
			);
		}

		/// <summary>
		///		Create a new HTTP request builder that is not attached to an <see cref="System.Net.Http.HttpClient"/>.
		/// </summary>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest Create(string requestUri)
		{
			if (String.IsNullOrWhiteSpace(requestUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'requestUri'.", nameof(requestUri));

			return Create(
				new Uri(requestUri, UriKind.RelativeOrAbsolute)
			);
		}

		/// <summary>
		///		Create a new HTTP request builder that is not attached to an <see cref="System.Net.Http.HttpClient"/>.
		/// </summary>
		/// <param name="requestUri">
		///		The request URI (can be relative or absolute).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public static HttpRequest Create(Uri requestUri)
		{
			if (requestUri == null)
				throw new ArgumentNullException(nameof(requestUri));

			RequestProperties.Builder properties = BaseProperties.ToBuilder();

			properties[nameof(RequestUri)] = requestUri;
			properties[nameof(IsUriTemplate)] = UriTemplate.IsTemplate(requestUri);

			return new HttpRequest(
				properties.ToImmutable()
			);
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		///		Actions (if any) to perform on the outgoing request message.
		/// </summary>
		public ImmutableList<RequestAction> RequestActions => GetProperty<ImmutableList<RequestAction>>();

		/// <summary>
		///     The request's URI template parameters (if any).
		/// </summary>
		public ImmutableDictionary<string, ISimpleValueProvider<string>> TemplateParameters => GetProperty<ImmutableDictionary<string, ISimpleValueProvider<string>>>();

		/// <summary>
		///     The request's query parameters (if any).
		/// </summary>
		public ImmutableDictionary<string, ISimpleValueProvider<string>> QueryParameters => GetProperty<ImmutableDictionary<string, ISimpleValueProvider<string>>>();

		#endregion // Properties

		#region IUntypedHttpRequest

		/// <summary>
		///		Actions (if any) to perform on the outgoing request message.
		/// </summary>
		IReadOnlyList<RequestAction> IUntypedHttpRequest.RequestActions => RequestActions;

		/// <summary>
		///     The request's URI template parameters (if any).
		/// </summary>
		IReadOnlyDictionary<string, ISimpleValueProvider<string>> IUntypedHttpRequest.TemplateParameters => TemplateParameters;

		/// <summary>
		///     The request's query parameters (if any).
		/// </summary>
		IReadOnlyDictionary<string, ISimpleValueProvider<string>> IUntypedHttpRequest.QueryParameters => QueryParameters;

		#endregion // IUntypedHttpRequest

		#region Invocation

		/// <summary>
		///		Build and configure a new HTTP request message.
		/// </summary>
		/// <param name="httpMethod">
		///		The HTTP request method to use.
		/// </param>
		/// <param name="body">
		///		Optional <see cref="HttpContent"/> representing the request body.
		/// </param>
		/// <param name="baseUri">
		///		An optional base URI to use if the request builder does not already have an absolute request URI.
		/// </param>
		/// <returns>
		///		The configured <see cref="HttpRequestMessage"/>.
		/// </returns>
		public HttpRequestMessage BuildRequestMessage(HttpMethod httpMethod, HttpContent body = null, Uri baseUri = null)
		{
			if (httpMethod == null)
				throw new ArgumentNullException(nameof(httpMethod));

			// Ensure we have an absolute URI.
			Uri requestUri = RequestUri;
			if (!requestUri.IsAbsoluteUri)
			{
				if (baseUri == null)
					throw new InvalidOperationException("Cannot build a request message; the request builder does not have an absolute request URI, and no base URI was supplied.");

				// Make relative to base URI.
				requestUri = baseUri.AppendRelativeUri(requestUri);
			}
			else
			{
				// Extract base URI to which request URI is already (by definition) relative.
				baseUri = new Uri(
					requestUri.GetComponents(
						UriComponents.Scheme | UriComponents.StrongAuthority,
						UriFormat.UriEscaped
					)
				);
			}

			if (IsUriTemplate)
			{
				UriTemplate template = new UriTemplate(
					requestUri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped)
				);

				IDictionary<string, string> templateParameterValues = GetTemplateParameterValues();

				requestUri = template.Populate(baseUri, templateParameterValues);
			}

			// Merge in any other query parameters defined directly on the request builder.
			requestUri = MergeQueryParameters(requestUri);

			HttpRequestMessage requestMessage = null;
			try
			{
				requestMessage = new HttpRequestMessage(httpMethod, requestUri);
				if (body != null)
					requestMessage.Content = body;

				List<Exception> configurationActionExceptions = new List<Exception>();
				foreach (RequestAction requestAction in RequestActions)
				{
					if (requestAction == null)
						continue;

					try
					{
						requestAction(requestMessage);
					}
					catch (Exception eConfigurationAction)
					{
						configurationActionExceptions.Add(eConfigurationAction);
					}
				}

				if (configurationActionExceptions.Count > 0)
				{
					throw new AggregateException(
						"One or more unhandled exceptions were encountered while configuring the outgoing request message.",
						configurationActionExceptions
					);
				}
			}
			catch
			{
				using (requestMessage)
				{
					throw;
				}
			}

			return requestMessage;
		}

		#endregion // Invocation

		#region Configuration

		/// <summary>
		///		Create a copy of the request builder with the specified base URI.
		/// </summary>
		/// <param name="baseUri">
		///		The request base URI.
		/// 
		///		Must be an absolute URI.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		The request builder already has an absolute URI.
		/// </exception>
		public HttpRequest WithBaseUri(Uri baseUri)
		{
			if (baseUri == null)
				throw new ArgumentNullException(nameof(baseUri));

			if (!baseUri.IsAbsoluteUri)
				throw new ArgumentException("The supplied base URI is not an absolute URI.", nameof(baseUri));

			if (RequestUri.IsAbsoluteUri)
				throw new InvalidOperationException("The request builder already has an absolute URI.");

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(RequestUri)] = baseUri.AppendRelativeUri(RequestUri);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI.
		/// </summary>
		/// <param name="requestUri">
		///		The new request URI.
		/// 
		///		Must be an absolute URI (otherwise, use <see cref="WithRelativeRequestUri(System.Uri)"/>).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithRequestUri(Uri requestUri)
		{
			if (requestUri == null)
				throw new ArgumentNullException(nameof(requestUri));

			if (!requestUri.IsAbsoluteUri)
				throw new ArgumentException("The specified URI is not an absolute URI.", nameof(requestUri));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(RequestUri)] = requestUri;
			}));
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI appended to its existing URI.
		/// </summary>
		/// <param name="relativeUri">
		///		The relative request URI.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithRelativeRequestUri(string relativeUri)
		{
			if (String.IsNullOrWhiteSpace(relativeUri))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'relativeUri'.", nameof(relativeUri));

			return WithRelativeRequestUri(
				new Uri(relativeUri, UriKind.Relative)
			);
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI appended to its existing URI.
		/// </summary>
		/// <param name="relativeUri">
		///		The relative request URI.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithRelativeRequestUri(Uri relativeUri)
		{
			if (relativeUri == null)
				throw new ArgumentNullException(nameof(relativeUri));

			if (relativeUri.IsAbsoluteUri)
				throw new ArgumentException("The specified URI is not a relative URI.", nameof(relativeUri));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(RequestUri)] = RequestUri.AppendRelativeUri(relativeUri);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request-configuration action.
		/// </summary>
		/// <param name="requestAction">
		///		A delegate that configures outgoing request messages.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithRequestAction(RequestAction requestAction)
		{
			if (requestAction == null)
				throw new ArgumentNullException(nameof(requestAction));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(RequestActions)] = RequestActions.Add(requestAction);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request-configuration actions.
		/// </summary>
		/// <param name="requestActions">
		///		A delegate that configures outgoing request messages.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithRequestAction(params RequestAction[] requestActions)
		{
			if (requestActions == null)
				throw new ArgumentNullException(nameof(requestActions));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(RequestActions)] = RequestActions.AddRange(requestActions);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI query parameter.
		/// </summary>
		/// <typeparam name="T">
		///		The parameter data-type.
		/// </typeparam>
		/// <param name="name">
		///		The parameter name.
		/// </param>
		/// <param name="valueProvider">
		///		Delegate that, given the current context, returns the parameter value (cannot be <c>null</c>).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithQueryParameter<T>(string name, ISimpleValueProvider<T> valueProvider)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(name));

			if (valueProvider == null)
				throw new ArgumentNullException(nameof(valueProvider));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(QueryParameters)] = QueryParameters.SetItem(
					key: name,
					value: valueProvider.Convert().ValueToString()
				);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI query parameter.
		/// </summary>
		/// <typeparam name="T">
		///		The parameter data-type.
		/// </typeparam>
		/// <param name="name">
		///		The parameter name.
		/// </param>
		/// <param name="getValue">
		///		Delegate that returns the parameter value (cannot be <c>null</c>).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithQueryParameter<T>(string name, Func<T> getValue)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(name));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return WithQueryParameter(
				name,
				SimpleValueProvider.FromFunction(getValue)
			);
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI query parameter.
		/// </summary>
		/// <param name="queryParameters">
		///		A sequence of 0 or more key / value pairs representing the query parameters (values cannot be <c>null</c>).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithQueryParameters(IEnumerable<KeyValuePair<string, ISimpleValueProvider<string>>> queryParameters)
		{
			if (queryParameters == null)
				throw new ArgumentNullException(nameof(queryParameters));

			bool modified = false;
			ImmutableDictionary<string, ISimpleValueProvider<string>>.Builder queryParametersBuilder = QueryParameters.ToBuilder();
			foreach (KeyValuePair<string, ISimpleValueProvider<string>> queryParameter in queryParameters)
			{
				if (queryParameter.Value == null)
				{
					throw new ArgumentException(
						String.Format(
							"Query parameter '{0}' has a null getter; this is not supported.",
							queryParameter.Key
						),
						nameof(queryParameters)
					);
				}

				queryParametersBuilder[queryParameter.Key] = queryParameter.Value;
				modified = true;
			}

			if (!modified)
				return this;

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(QueryParameters)] = queryParametersBuilder.ToImmutable();
			}));
		}

		/// <summary>
		///		Create a copy of the request builder without the specified request URI query parameter.
		/// </summary>
		/// <param name="name">
		///		The parameter name.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithoutQueryParameter(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(name));

			if (!QueryParameters.ContainsKey(name))
				return this;

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(QueryParameters)] = QueryParameters.Remove(name);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder without the specified request URI query parameters.
		/// </summary>
		/// <param name="names">
		///		The parameter names.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithoutQueryParameters(IEnumerable<string> names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(QueryParameters)] = QueryParameters.RemoveRange(names);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI template parameter.
		/// </summary>
		/// <typeparam name="T">
		///		The parameter data-type.
		/// </typeparam>
		/// <param name="name">
		///		The parameter name.
		/// </param>
		/// <param name="valueProvider">
		///		A <see cref="IValueProvider{TSource, TValue}">value provider</see> that, given the current context, returns the parameter value.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithTemplateParameter<T>(string name, ISimpleValueProvider<T> valueProvider)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(name));

			if (valueProvider == null)
				throw new ArgumentNullException(nameof(valueProvider));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(TemplateParameters)] = TemplateParameters.SetItem(
					key: name,
					value: valueProvider.Convert().ValueToString()
				);
			}));
		}
		
		/// <summary>
		///		Create a copy of the request builder with the specified request URI template parameter.
		/// </summary>
		/// <typeparam name="T">
		///		The parameter data-type.
		/// </typeparam>
		/// <param name="name">
		///		The parameter name.
		/// </param>
		/// <param name="getValue">
		///		Delegate that returns the parameter value (cannot be <c>null</c>).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithTemplateParameter<T>(string name, Func<T> getValue)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(name));

			if (getValue == null)
				throw new ArgumentNullException(nameof(getValue));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(TemplateParameters)] = TemplateParameters.SetItem(
					key: name,
					value: SimpleValueProvider.FromFunction(getValue).Convert().ValueToString()
				);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder with the specified request URI template parameter.
		/// </summary>
		/// <param name="templateParameters">
		///		A sequence of 0 or more key / value pairs representing the template parameters (values cannot be <c>null</c>).
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithTemplateParameters(IEnumerable<KeyValuePair<string, ISimpleValueProvider<string>>> templateParameters)
		{
			if (templateParameters == null)
				throw new ArgumentNullException(nameof(templateParameters));

			bool modified = false;
			ImmutableDictionary<string, ISimpleValueProvider<string>>.Builder templateParametersBuilder = TemplateParameters.ToBuilder();
			foreach (KeyValuePair<string, ISimpleValueProvider<string>> templateParameter in templateParameters)
			{
				if (templateParameter.Value == null)
				{
					throw new ArgumentException(
						String.Format(
							"Template parameter '{0}' has a null getter; this is not supported.",
							templateParameter.Key
						),
						nameof(templateParameters)
					);
				}

				templateParametersBuilder[templateParameter.Key] = templateParameter.Value;
				modified = true;
			}

			if (!modified)
				return this;

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(TemplateParameters)] = templateParametersBuilder.ToImmutable();
			}));
		}

		/// <summary>
		///		Create a copy of the request builder without the specified request URI template parameter.
		/// </summary>
		/// <param name="name">
		///		The parameter name.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithoutTemplateParameter(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'name'.", nameof(name));

			if (!TemplateParameters.ContainsKey(name))
				return this;

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(TemplateParameters)] = TemplateParameters.Remove(name);
			}));
		}

		/// <summary>
		///		Create a copy of the request builder without the specified request URI template parameters.
		/// </summary>
		/// <param name="names">
		///		The parameter names.
		/// </param>
		/// <returns>
		///		The new <see cref="HttpRequest"/>.
		/// </returns>
		public HttpRequest WithoutTemplateParameters(IEnumerable<string> names)
		{
			if (names == null)
				throw new ArgumentNullException(nameof(names));

			return new HttpRequest(CloneProperties(properties =>
			{
				properties[nameof(TemplateParameters)] = TemplateParameters.RemoveRange(names);
			}));
		}

		#endregion // Configuration

		#region Helpers

		/// <summary>
		///		Merge the request builder's query parameters (if any) into the request URI.
		/// </summary>
		/// <param name="requestUri">
		///		The request URI.
		/// </param>
		/// <returns>
		///		The request URI with query parameters merged into it.
		/// </returns>
		Uri MergeQueryParameters(Uri requestUri)
		{
			if (requestUri == null)
				throw new ArgumentNullException(nameof(requestUri));

			if (QueryParameters.Count == 0)
				return requestUri;

			NameValueCollection queryParameters = requestUri.ParseQueryParameters();
			foreach (KeyValuePair<string, ISimpleValueProvider<string>> queryParameter in QueryParameters)
			{
				string queryParameterValue = queryParameter.Value.Get();
				if (queryParameterValue != null)
					queryParameters[queryParameter.Key] = queryParameterValue;
				else
					queryParameters.Remove(queryParameter.Key);
			}

			return requestUri.WithQueryParameters(queryParameters);
		}

		/// <summary>
		///		Get a dictionary mapping template parameters (if any) to their current values.
		/// </summary>
		/// <returns>
		///		A dictionary of key / value pairs (any parameters whose value-getters return null will be omitted).
		/// </returns>
		IDictionary<string, string> GetTemplateParameterValues()
		{
			return
				TemplateParameters.Select(templateParameter =>
				{
					Debug.Assert(templateParameter.Value != null);

					return new
					{
						templateParameter.Key,
						Value = templateParameter.Value.Get()
					};
				})
				.Where(
					templateParameter => templateParameter.Value != null
				)
				.ToDictionary(
					templateParameter => templateParameter.Key,
					templateParameter => templateParameter.Value
				);
		}

		#endregion // Helpers
	}
}

