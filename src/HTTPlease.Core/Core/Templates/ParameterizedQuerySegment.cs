using System;
using System.Text;

namespace HTTPlease.Core.Templates
{
    /// <summary>
    ///    A template segment that represents a query parameter whose value comes from a template parameter.
    /// </summary>
    sealed class ParameterizedQuerySegment
        : QuerySegment
    {
        /// <summary>
        ///    The name of the template parameter whose value becomes the query parameter.
        /// </summary>
        readonly string    _templateParameterName;

        /// <summary>
        ///    Is the segment optional?
        /// </summary>
        /// <remarks>
        ///    If <c>true</c>, then the query parameter will be omitted if its associated template variable is not defined.
        /// </remarks>
        readonly bool    _isOptional;

        /// <summary>
        ///    Create a new literal query segment.
        /// </summary>
        /// <param name="queryParameterName">
        ///    The name of the query parameter that the segment represents.
        /// </param>
        /// <param name="templateParameterName">
        ///    The value for the query parameter that the segment represents.
        /// </param>
        /// <param name="isOptional">
        ///    Is the segment optional?
        /// </param>
        public ParameterizedQuerySegment(string queryParameterName, string templateParameterName, bool isOptional = false)
            : base(queryParameterName)
        {
            if (String.IsNullOrWhiteSpace(templateParameterName))
                throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'value'.", nameof(templateParameterName));

            _templateParameterName = templateParameterName;
            _isOptional = isOptional;
        }

        /// <summary>
        ///    The name of the template parameter whose value becomes the query parameter.
        /// </summary>
        public string TemplateParameterName
        {
            get
            {
                return _templateParameterName;
            }
        }

        /// <summary>
        ///    Is the segment optional?
        /// </summary>
        /// <remarks>
        ///    If <c>true</c>, then the query parameter will be omitted if its associated template variable is not defined.
        /// </remarks>
        public bool IsOptional
        {
            get
            {
                return _isOptional;
            }
        }

        /// <summary>
        ///    Does the segment have a parameterised (non-constant) value?
        /// </summary>
        public override bool IsParameterized => true;

        /// <summary>
        /// Render the template segment as text.
        /// </summary>
        /// <param name="output">The <see cref="StringBuilder"/> to which the rendered text will be appended.</param>
        /// <param name="evaluationContext">The template evaluation context.</param>
        /// <returns><c>true</c>, if the segment produced any output; otherwise, <c>false</c>.</returns>
        public override bool Render(StringBuilder output, ITemplateEvaluationContext evaluationContext)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            
            if (evaluationContext == null)
                throw new ArgumentNullException(nameof(evaluationContext));
            
            string parameterValue = evaluationContext[_templateParameterName, _isOptional];
            if (parameterValue == null)
                return false;

            output.Append(Name);

            if (parameterValue != String.Empty)
            {
                output.Append('=');
                output.Append(
                    Escape(parameterValue)
                );
            }

            return true;
        }
    }
}
