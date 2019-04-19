﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPlease
{
    using Core.Templates;

    /// <summary>
    ///    Populates parameterised URI templates.
    /// </summary>
    public sealed class UriTemplate
    {
        /// <summary>
        ///    The URI template.
        /// </summary>
        readonly string                            _template;

        /// <summary>
        ///    The template's URI segments.
        /// </summary>
        readonly IReadOnlyList<UriSegment>        _uriSegments;

        /// <summary>
        ///    The template's URI segments.
        /// </summary>
        readonly IReadOnlyList<QuerySegment>    _querySegments;

        /// <summary>
        ///    Create a new URI template.
        /// </summary>
        /// <param name="template">
        ///    The template.
        /// </param>
        public UriTemplate(string template)
        {
            if (String.IsNullOrWhiteSpace(template))
                throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'template'.", nameof(template));

            _template = template;

            IReadOnlyList<TemplateSegment> templateSegments = TemplateSegment.Parse(_template);
            _uriSegments = templateSegments.OfType<UriSegment>().ToArray();
            if (_uriSegments.Count == 0)
                throw new UriTemplateException("Invalid URI template (contains no path segments).");

            _querySegments = templateSegments.OfType<QuerySegment>().ToArray();
        }

        /// <summary>
        ///    Build a URI from the template.
        /// </summary>
        /// <param name="templateParameters">
        ///    A dictionary containing the template parameters.
        /// </param>
        /// <returns>
        ///    The generated URI.
        /// </returns>
        public Uri Populate(IDictionary<string, string> templateParameters)
        {
            return Populate(null, templateParameters);
        }

        /// <summary>
        ///    Build a URI from the template.
        /// </summary>
        /// <param name="baseUri">
        ///    The base URI, or <c>null</c> to generate a relative URI.
        /// </param>
        /// <param name="templateParameters">
        ///    A dictionary containing the template parameters.
        /// </param>
        /// <returns>
        ///    The generated URI.
        /// </returns>
        public Uri Populate(Uri baseUri, IDictionary<string, string> templateParameters)
        {
            if (baseUri != null && !baseUri.IsAbsoluteUri)
                throw new UriTemplateException("Base URI '{0}' is not an absolute URI.", baseUri);

            if (templateParameters == null)
                throw new ArgumentNullException(nameof(templateParameters));

            TemplateEvaluationContext evaluationContext = new TemplateEvaluationContext(templateParameters);
            StringBuilder uriBuilder = new StringBuilder();
            if (baseUri != null)
            {
                uriBuilder.Append(
                    baseUri.GetComponents(UriComponents.Scheme | UriComponents.StrongAuthority, UriFormat.UriEscaped)
                );
            }

            if (_uriSegments.Count > 0)
            {
                foreach (UriSegment uriSegment in _uriSegments)
                    uriSegment.Render(uriBuilder, evaluationContext);
            }
            else
                uriBuilder.Append('/');

            if (_querySegments.Count > 0)
            {
                int queryStartIndex = uriBuilder.Length;

                bool renderedAny = false;
                foreach (QuerySegment querySegment in _querySegments)
                {
                    int parameterStartIndex = uriBuilder.Length;

                    bool rendered = querySegment.Render(uriBuilder, evaluationContext);
                    if (rendered && renderedAny)
                    {
                        // Insert the leading '&' for this query parameter.
                        uriBuilder.Insert(parameterStartIndex, '&');
                    }
                    else
                        renderedAny |= rendered;

                }

                if (renderedAny)
                {
                    // Insert the leading '?'.
                    uriBuilder.Insert(queryStartIndex, '?');
                }
            }

            return new Uri(uriBuilder.ToString(), UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        ///    Does the specified URI represent a template?
        /// </summary>
        /// <param name="uri">
        ///    The URI.
        /// </param>
        /// <returns>
        ///    <c>true</c>, if any of the URI's components are parameterised (i.e. have non-constant values); otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTemplate(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            return IsTemplate(uri.ToString());
        }

        /// <summary>
        ///    Does the specified URI represent a template?
        /// </summary>
        /// <param name="uri">
        ///    The URI.
        /// </param>
        /// <returns>
        ///    <c>true</c>, if any of the URI's components are parameterised (i.e. have non-constant values); otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTemplate(string uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            IReadOnlyList<TemplateSegment> templateSegments = TemplateSegment.Parse(uri);

            return templateSegments.Any(segment => segment.IsParameterized);
        }
    }
}
