using System;
using System.Text;

namespace HTTPlease.Core.Templates
{
    /// <summary>
    ///    A template segment that represents a literal query parameter (i.e. one that has a constant value).
    /// </summary>
    sealed class LiteralQuerySegment
        : QuerySegment
    {
        /// <summary>
        ///    The value for the query parameter that the segment represents.
        /// </summary>
        readonly string _value;

        /// <summary>
        ///    Create a new literal query segment.
        /// </summary>
        /// <param name="queryParameterName">
        ///    The name of the query parameter that the segment represents.
        /// </param>
        /// <param name="queryParameterValue">
        ///    The value for the query parameter that the segment represents.
        /// </param>
        public LiteralQuerySegment(string queryParameterName, string queryParameterValue)
            : base(queryParameterName)
        {
            if (String.IsNullOrWhiteSpace(queryParameterValue))
                throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'value'.", nameof(queryParameterValue));

            _value = queryParameterValue;
        }

        /// <summary>
        ///    The value for the query parameter that the segment represents.
        /// </summary>
        public string QueryParameterValue
        {
            get
            {
                return _value;
            }
        }

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
            
            if (_value == null)
                return false;

            output.Append(Name);

            if (_value != String.Empty)
            {
                output.Append('=');
                output.Append(
                    Escape(_value)
                );
            }

            return true;
        }
    }
}
