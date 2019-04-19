using System;
using System.Text;

namespace HTTPlease.Core.Templates
{
    /// <summary>
    ///    Represents a literal URI segment (i.e. one that has a constant value).
    /// </summary>
    sealed class ParameterizedUriSegment
        : UriSegment
    {
        /// <summary>
        ///    The name of the parameter from which the URI segment obtains its value.
        /// </summary>
        readonly string _templateParameterName;

        /// <summary>
        ///    Is the segment optional?
        /// </summary>
        /// <remarks>
        ///    If <c>true</c>, then the segment is not rendered when its associated parameter is missing.
        /// </remarks>
        readonly bool    _isOptional;

        /// <summary>
        ///    Create a new literal URI segment.
        /// </summary>
        /// <param name="templateParameterName">
        ///    The name of the parameter from which the URI segment obtains its value.
        /// </param>
        /// <param name="isDirectory">
        ///    Does the segment represent a directory (i.e. have a trailing slash?).
        /// </param>
        /// <param name="isOptional">
        ///    Is the segment optional?
        /// 
        ///    Default is <c>false</c>.
        /// </param>
        public ParameterizedUriSegment(string templateParameterName, bool isDirectory, bool isOptional = false)
            : base(isDirectory)
        {
            if (String.IsNullOrWhiteSpace(templateParameterName))
                throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'value'.", nameof(templateParameterName));

            _templateParameterName = templateParameterName;
            _isOptional = isOptional;
        }

        /// <summary>
        ///    The name of the parameter from which the URI segment obtains its value.
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
        ///    If <c>true</c>, then the segment is not rendered when its associated parameter is missing.
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
        /// <param name="output">
        ///     The <see cref="StringBuilder"/> to which the rendered text will be appended.
        /// </param>
        /// <param name="evaluationContext">
        ///     The template evaluation context.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if the segment produced any output; otherwise, <c>false</c>.
        /// </returns>
        public override bool Render(StringBuilder output, ITemplateEvaluationContext evaluationContext)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            
            if (evaluationContext == null)
                throw new ArgumentNullException(nameof(evaluationContext));
            
            string parameterValue = evaluationContext[_templateParameterName, _isOptional];
            if (parameterValue == null)
                return false;

            output.Append(
                Escape(parameterValue)
            );

            if (IsDirectory)
                output.Append('/');

            return true;
        }
    }
}
