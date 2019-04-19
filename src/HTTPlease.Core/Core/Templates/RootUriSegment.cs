using System;
using System.Text;

namespace HTTPlease.Core.Templates
{
    /// <summary>
    ///    A literal URI segment representing the root folder ("/").
    /// </summary>
    sealed class RootUriSegment
        : UriSegment
    {
        /// <summary>
        ///    The singleton instance of the root URI segment.
        /// </summary>
        public static readonly RootUriSegment Instance = new RootUriSegment();

        /// <summary>
        ///    Create a new literal URI segment.
        /// </summary>
        RootUriSegment()
            : base(isDirectory: true)
        {
        }

        /// <summary>
        /// Render the template segment as text.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/> to which the rendered text will be appended.</param>
        /// <param name="evaluationContext">The template evaluation context.</param>
        /// <returns><c>true</c>, if the segment produced any output; otherwise, <c>false</c>.</returns>
        public override bool Render(StringBuilder stringBuilder, ITemplateEvaluationContext evaluationContext)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException(nameof(stringBuilder));
            
            if (evaluationContext == null)
                throw new ArgumentNullException(nameof(evaluationContext));
            
            stringBuilder.Append('/');

            return false;
        }
    }
}
