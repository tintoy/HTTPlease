using System.Collections.Generic;

namespace HTTPlease.Core.ValueProviders
{
    /// <summary>
    ///    Represents the provider for a value from an instance of <typeparamref name="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">
    ///    The source type from which the value is extracted.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///    The type of value returned by the provider.
    /// </typeparam>
    public interface IValueProvider<in TContext, out TValue>
    {
        /// <summary>
        ///    Extract a value from the specified context.
        /// </summary>
        /// <param name="source">    
        ///    The <typeparamref name="TContext"/> instance from which the value is to be extracted.
        /// </param>
        /// <returns>
        ///    The value.
        /// </returns>
        TValue Get(TContext source);

        /// <summary>
        ///    Extract values from the specified context.
        /// </summary>
        /// <param name="source">    
        ///    The TContext instance from which the values are to be extracted.
        /// </param>
        /// <returns>
        ///    The values.
        /// </returns>
        IEnumerable<TValue> GetMultiple(TContext source);
    }
}
