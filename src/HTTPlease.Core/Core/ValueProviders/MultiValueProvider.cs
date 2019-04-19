using System;
using System.Collections.Generic;
using System.Linq;

namespace HTTPlease.Core.ValueProviders
{
	/// <summary>
	///		Factory methods for creating multi-value providers.
	/// </summary>
	/// <typeparam name="TContext">
	///		The type used as a context for each request.
	/// </typeparam>
	public static class MultiValueProvider<TContext>
	{
		/// <summary>
		///		Create a value provider from the specified selector function.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of values returned by the selector.
		/// </typeparam>
		/// <param name="selector">
		///		A selector function that, when given an instance of <typeparamref name="TContext"/>, returns values of type <typeparamref name="TValue"/> derived from the context.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static IValueProvider<TContext, TValue> FromSelector<TValue>(Func<TContext, IEnumerable<TValue>> selector)
		{
			if (selector == null)
				throw new ArgumentNullException(nameof(selector));

			return new SelectorMultiValueProvider<TValue>(selector);
		}

		/// <summary>
		///		Create a value provider from the specified function.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of values returned by the function.
		/// </typeparam>
		/// <param name="getValues">
		///		A function that returns values of type <typeparamref name="TValue"/>.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static IValueProvider<TContext, TValue> FromFunction<TValue>(Func<IEnumerable<TValue>> getValues)
		{
			if (getValues == null)
				throw new ArgumentNullException(nameof(getValues));

			return new FunctionMultiValueProvider<TValue>(getValues);
		}

		/// <summary>
		///		Create a value provider from the specified constant values.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		/// <param name="values">
		///		The constant values that are returned by the provider.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static IValueProvider<TContext, TValue> FromConstantValues<TValue>(IEnumerable<TValue> values)
		{
			return new StaticMultiValueProvider<TValue>(values);
		}

		/// <summary>
		///		A multi-value provider that invokes a selector function on the context to extract its values.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		class SelectorMultiValueProvider<TValue>
			: IValueProvider<TContext, TValue>
		{
			/// <summary>
			///		The selector function that extracts values from the context.
			/// </summary>
			readonly Func<TContext, IEnumerable<TValue>> _selector;

			/// <summary>
			///		Create a new selector-based value provider.
			/// </summary>
			/// <param name="selector">
			///		The selector function that extracts a value from the context.
			/// </param>
			public SelectorMultiValueProvider(Func<TContext, IEnumerable<TValue>> selector)
			{
				if (selector == null)
					throw new ArgumentNullException(nameof(selector));

				_selector = selector;
			}

			/// <summary>
			///		Extract values from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The TContext instance from which the values are to be extracted.
			/// </param>
			/// <returns>
			///		The values.
			/// </returns>
			public IEnumerable<TValue> GetMultiple(TContext source)
			{
				if (source == null)
					throw new InvalidOperationException("The current request template has one more more deferred parameters that refer to its context; the context parameter must therefore be supplied.");

				return _selector(source);
			}

			/// <summary>
			///		Extract a value from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The TContext instance from which the value is to be extracted.
			/// </param>
			/// <returns>
			///		The value.
			/// </returns>
			public TValue Get(TContext source) => GetMultiple(source).FirstOrDefault();
		}

		/// <summary>
		///		A multi-value provider that invokes a function to extract its value.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		class FunctionMultiValueProvider<TValue>
			: IValueProvider<TContext, TValue>
		{
			/// <summary>
			///		The function that is invoked to provide a value.
			/// </summary>
			readonly Func<IEnumerable<TValue>> _getValues;

			/// <summary>
			///		Create a new function-based value provider.
			/// </summary>
			/// <param name="getValues">
			///		The function that is invoked to provide values.
			/// </param>
			public FunctionMultiValueProvider(Func<IEnumerable<TValue>> getValues)
			{
				if (getValues == null)
					throw new ArgumentNullException(nameof(getValues));

				_getValues = getValues;
			}

			/// <summary>
			///		Extract values from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The TContext instance from which the values are to be extracted.
			/// </param>
			/// <returns>
			///		The values.
			/// </returns>
			public IEnumerable<TValue> GetMultiple(TContext source)
			{
				if (source == null)
					return Enumerable.Empty<TValue>();

				return _getValues();
			}

			/// <summary>
			///		Extract a value from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The TContext instance from which the value is to be extracted.
			/// </param>
			/// <returns>
			///		The value.
			/// </returns>
			public TValue Get(TContext source) => GetMultiple(source).FirstOrDefault();
		}

		/// <summary>
		///		A multi-value provider that returns a constant value.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		class StaticMultiValueProvider<TValue>
			: IValueProvider<TContext, TValue>
		{
			/// <summary>
			///		The values returned by the provider.
			/// </summary>
			readonly IEnumerable<TValue> _values;

			/// <summary>
			///		Create a new constant value provider.
			/// </summary>
			/// <param name="values">
			///		The values returned by the provider.
			/// </param>
			public StaticMultiValueProvider(IEnumerable<TValue> values)
			{
				if (values == null)
					throw new ArgumentNullException(nameof(values));

				_values = values;
			}

			/// <summary>
			///		Extract values from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The TContext instance from which the values are to be extracted.
			/// </param>
			/// <returns>
			///		The values.
			/// </returns>
			public IEnumerable<TValue> GetMultiple(TContext source) => _values;

			/// <summary>
			///		Extract a value from the specified context.
			/// </summary>
			/// <param name="source">	
			///		The TContext instance from which the value is to be extracted.
			/// </param>
			/// <returns>
			///		The value.
			/// </returns>
			public TValue Get(TContext source) => GetMultiple(source).FirstOrDefault();
		}
	}
}
