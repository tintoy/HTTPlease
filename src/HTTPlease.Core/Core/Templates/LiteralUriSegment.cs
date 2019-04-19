using System;
using System.Text;

namespace HTTPlease.Core.Templates
{
    /// <summary>
    ///    Represents a literal URI segment (i.e. one that has a constant value).
    /// </summary>
    sealed class LiteralUriSegment
        : UriSegment
    {
        /// <summary>
        ///    The segment value;
        /// </summary>
        readonly string _value;

        /// <summary>
        ///    Create a new literal URI segment.
        /// </summary>
        /// <param name="value">
        ///    The segment value.
        /// </param>
        /// <param name="isDirectory">
        ///    Does the segment represent a directory (i.e. have a trailing slash?).
        /// </param>
        public LiteralUriSegment(string value, bool isDirectory)
            : base(isDirectory)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'value'.", nameof(value));

            _value = value;
        }

        /// <summary>
        ///    The segment value;
        /// </summary>
        public string Value
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
            
            output.Append(_value);

            if (IsDirectory)
                output.Append('/');

            return true;
        }
    }
}
