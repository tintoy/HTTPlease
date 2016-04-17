using System;

namespace HTTPlease.ValueProviders
{
	/// <summary>
	///		Conversion operations for a simple value provider.
	/// </summary>
	/// <typeparam name="TValue">
	///		The type of value returned by the value provider.
	/// </typeparam>
	/// <remarks>
	///		This conversion enables base requests to be defined without a context type, and then more-derived requests can be specialised for their specific context types.
	/// </remarks>
	public struct SimpleValueProviderConversion<TValue>
	{
		/// <summary>
		///		Create a new value-provider conversion.
		/// </summary>
		/// <param name="valueProvider">
		///		The value provider being converted.
		/// </param>
		public SimpleValueProviderConversion(ISimpleValueProvider<TValue> valueProvider)
			: this()
		{
			if (valueProvider == null)
				throw new ArgumentNullException(nameof(valueProvider));

			ValueProvider = valueProvider;
		}

		/// <summary>
		///		The value provider being converted.
		/// </summary>
		public ISimpleValueProvider<TValue> ValueProvider { get; }

		/// <summary>
		///		Wrap the specified value provider in a value provider that utilises a specific context type.
		/// </summary>
		/// <typeparam name="TContext">
		///		The type used by the new provider as a context for each request.
		/// </typeparam>
		/// <returns>
		///		The outer (converting) value provider.
		/// </returns>
		public IContextValueProvider<TContext, TValue> ContextTo<TContext>()
		{
			// Can't close over members of structs.
			ISimpleValueProvider<TValue> valueProvider = ValueProvider;

			return ValueProvider<TContext>.FromSelector(
				context => valueProvider.Get()
			);
		}

		/// <summary>
		///		Wrap the value provider in a value provider that converts its value to a string.
		/// </summary>
		/// <returns>
		///		The outer (converting) value provider.
		/// </returns>
		/// <remarks>
		///		If the underlying value is <c>null</c> then the converted string value will be <c>null</c>, too.
		/// </remarks>
		public ISimpleValueProvider<string> ValueToString()
		{
			// Special-case conversion to save on allocations.
			if (typeof(TValue) == typeof(string))
				return (IValueProvider<Unit, string>)ValueProvider;

			// Can't close over members of structs.
			ISimpleValueProvider<TValue> valueProvider = ValueProvider;

			return ValueProvider<Unit>.FromFunction(() =>
			{
				TValue value = valueProvider.Get();

				return value != null ? value.ToString() : null;
			});
		}
	}
}