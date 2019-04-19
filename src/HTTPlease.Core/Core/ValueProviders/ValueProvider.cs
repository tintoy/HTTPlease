﻿using System;
using System.Collections.Generic;

namespace HTTPlease.Core.ValueProviders
{
    /// <summary>
    ///    Factory methods for creating value providers.
    /// </summary>
    /// <typeparam name="TContext">
    ///    The type used as a context for each request.
    /// </typeparam>
    public static class ValueProvider<TContext>
    {
        /// <summary>
        ///    Create a value provider from the specified selector function.
        /// </summary>
        /// <typeparam name="TValue">
        ///    The type of value returned by the selector.
        /// </typeparam>
        /// <param name="selector">
        ///    A selector function that, when given an instance of <typeparamref name="TContext"/>, returns a value of type <typeparamref name="TValue"/> derived from the context.
        /// </param>
        /// <returns>
        ///    The value provider.
        /// </returns>
        public static IValueProvider<TContext, TValue> FromSelector<TValue>(Func<TContext, TValue> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new SelectorValueProvider<TValue>(selector);
        }

        /// <summary>
        ///    Create a value provider from the specified function.
        /// </summary>
        /// <typeparam name="TValue">
        ///    The type of value returned by the function.
        /// </typeparam>
        /// <param name="getValue">
        ///    A function that returns a value of type <typeparamref name="TValue"/>.
        /// </param>
        /// <returns>
        ///    The value provider.
        /// </returns>
        public static IValueProvider<TContext, TValue> FromFunction<TValue>(Func<TValue> getValue)
        {
            if (getValue == null)
                throw new ArgumentNullException(nameof(getValue));

            return new FunctionValueProvider<TValue>(getValue);
        }

        /// <summary>
        ///    Create a value provider from the specified constant value.
        /// </summary>
        /// <typeparam name="TValue">
        ///    The type of value returned by the provider.
        /// </typeparam>
        /// <param name="value">
        ///    A constant value that is returned by the provider.
        /// </param>
        /// <returns>
        ///    The value provider.
        /// </returns>
        public static IValueProvider<TContext, TValue> FromConstantValue<TValue>(TValue value)
        {
            return new StaticValueProvider<TValue>(value);
        }

        /// <summary>
        ///    Value provider that invokes a selector function on the context to extract its value.
        /// </summary>
        /// <typeparam name="TValue">
        ///    The type of value returned by the provider.
        /// </typeparam>
        class SelectorValueProvider<TValue>
            : IValueProvider<TContext, TValue>
        {
            /// <summary>
            ///    The selector function that extracts a value from the context.
            /// </summary>
            readonly Func<TContext, TValue> _selector;

            /// <summary>
            ///    Create a new selector-based value provider.
            /// </summary>
            /// <param name="selector">
            ///    The selector function that extracts a value from the context.
            /// </param>
            public SelectorValueProvider(Func<TContext, TValue> selector)
            {
                if (selector == null)
                    throw new ArgumentNullException(nameof(selector));

                _selector = selector;
            }

            /// <summary>
            ///    Extract the value from the specified context.
            /// </summary>
            /// <param name="source">    
            ///    The TContext instance from which the value is to be extracted.
            /// </param>
            /// <returns>
            ///    The value.
            /// </returns>
            public TValue Get(TContext source)
            {
                if (source == null)
                    throw new InvalidOperationException("The current request template has one more more deferred parameters that refer to its context; the context parameter must therefore be supplied.");

                return _selector(source);
            }

            /// <summary>
            ///    Extract values from the specified context.
            /// </summary>
            /// <param name="source">    
            ///    The TContext instance from which the values are to be extracted.
            /// </param>
            /// <returns>
            ///    The values.
            /// </returns>
            public IEnumerable<TValue> GetMultiple(TContext source)
            {
                yield return Get(source);
            }
        }

        /// <summary>
        ///    Value provider that invokes a function to extract its value.
        /// </summary>
        /// <typeparam name="TValue">
        ///    The type of value returned by the provider.
        /// </typeparam>
        class FunctionValueProvider<TValue>
            : IValueProvider<TContext, TValue>
        {
            /// <summary>
            ///    The function that is invoked to provide a value.
            /// </summary>
            readonly Func<TValue> _getValue;

            /// <summary>
            ///    Create a new function-based value provider.
            /// </summary>
            /// <param name="getValue">
            ///    The function that is invoked to provide a value.
            /// </param>
            public FunctionValueProvider(Func<TValue> getValue)
            {
                if (getValue == null)
                    throw new ArgumentNullException(nameof(getValue));

                _getValue = getValue;
            }

            /// <summary>
            ///    Extract a value from the specified context.
            /// </summary>
            /// <param name="source">    
            ///    The TContext instance from which the value is to be extracted.
            /// </param>
            /// <returns>
            ///    The value.
            /// </returns>
            public TValue Get(TContext source) => _getValue();

            /// <summary>
            ///    Extract values from the specified context.
            /// </summary>
            /// <param name="source">    
            ///    The TContext instance from which the values are to be extracted.
            /// </param>
            /// <returns>
            ///    The values.
            /// </returns>
            public IEnumerable<TValue> GetMultiple(TContext source)
            {
                yield return Get(source);
            }
        }

        /// <summary>
        ///    Value provider that returns a static set of values.
        /// </summary>
        /// <typeparam name="TValue">
        ///    The type of value returned by the provider.
        /// </typeparam>
        class StaticValueProvider<TValue>
            : IValueProvider<TContext, TValue>
        {
            /// <summary>
            ///    The value returned by the provider.
            /// </summary>
            readonly TValue _value;

            /// <summary>
            ///    Create a new constant value provider.
            /// </summary>
            /// <param name="value">
            ///    The value returned by the provider.
            /// </param>
            public StaticValueProvider(TValue value)
            {
                _value = value;
            }

            /// <summary>
            ///    Extract the value from the specified context.
            /// </summary>
            /// <param name="source">    
            ///    The TContext instance from which the value is to be extracted.
            /// </param>
            /// <returns>
            ///    The value.
            /// </returns>
            public TValue Get(TContext source) => _value;

            /// <summary>
            ///    Extract values from the specified context.
            /// </summary>
            /// <param name="source">    
            ///    The TContext instance from which the values are to be extracted.
            /// </param>
            /// <returns>
            ///    The values.
            /// </returns>
            public IEnumerable<TValue> GetMultiple(TContext source)
            {
                yield return Get(source);
            }
        }
    }
}
