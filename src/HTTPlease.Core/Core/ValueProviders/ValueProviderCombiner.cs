using System;
using System.Collections.Generic;
using System.Linq;

namespace HTTPlease.Core.ValueProviders
{
    /// <summary>
    ///    Combination operations for a value provider.
    /// </summary>
    /// <typeparam name="TContext">
    ///    The type used as a context for each request.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///    The type of value returned by the value provider.
    /// </typeparam>
    public struct ValueProviderCombiner<TContext, TValue>
    {
        /// <summary>
        ///    Create a new value-provider combiner.
        /// </summary>
        /// <param name="valueProvider">
        ///    The value provider being combined.
        /// </param>
        public ValueProviderCombiner(IValueProvider<TContext, TValue> valueProvider)
            : this()
        {
            if (valueProvider == null)
                throw new ArgumentNullException(nameof(valueProvider));

            ValueProvider = valueProvider;
        }

        /// <summary>
        ///    The value provider being combined.
        /// </summary>
        public IValueProvider<TContext, TValue> ValueProvider { get; }

        /// <summary>
        ///    Wrap the value provider in a value provider that appends its values to the values supplied by another specified value provider.
        /// </summary>
        /// <returns>
        ///    The value provider whose values come first.
        /// </returns>
        public IValueProvider<TContext, TValue> ByAppendingTo(IValueProvider<TContext, TValue> valueProvider )
        {
            // Can't close over members of structs.
            IValueProvider<TContext, TValue> existingValueProvider = ValueProvider;

            return MultiValueProvider<TContext>.FromSelector(context =>
            {
                IEnumerable<TValue> existingValues = valueProvider.GetMultiple(context);
                IEnumerable<TValue> appendValues = existingValueProvider.GetMultiple(context);

                return existingValues.Concat(appendValues);
            });
        }
    }
}