using System;

namespace HTTPlease.ValueProviders
{
	/// <summary>
	///		Factory for value providers that do not depend on specific contexts.
	/// </summary>
    public static class SimpleValueProvider
    {
		/// <summary>
		///		Create a value provider from the specified function.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the function.
		/// </typeparam>
		/// <param name="getValue">
		///		A function that returns a well-known value of type <typeparamref name="TValue"/>.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static ISimpleValueProvider<TValue> FromFunction<TValue>(Func<TValue> getValue)
		{
			return ValueProvider<Unit>.FromFunction(getValue);
		}

		/// <summary>
		///		Create a value provider from the specified function that converts the function's return value into a string.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the function.
		/// </typeparam>
		/// <param name="getValue">
		///		A function that returns a well-known value of type <typeparamref name="TValue"/>.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static ISimpleValueProvider<string> StringFromFunction<TValue>(Func<TValue> getValue)
		{
			return ValueProvider<Unit>.FromFunction(getValue).ConvertSimple().ValueToString();
		}

		/// <summary>
		///		Create a value provider from the specified constant value.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		/// <param name="value">
		///		A constant value that is returned by the provider.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static ISimpleValueProvider<TValue> FromConstantValue<TValue>(TValue value)
		{
			return ValueProvider<Unit>.FromConstantValue(value);
		}

		/// <summary>
		///		Create a value provider that returns a string from the specified constant value.
		/// </summary>
		/// <typeparam name="TValue">
		///		The type of value returned by the provider.
		/// </typeparam>
		/// <param name="value">
		///		A constant value that is returned by the provider.
		/// </param>
		/// <returns>
		///		The value provider.
		/// </returns>
		public static ISimpleValueProvider<string> StringFromConstantValue<TValue>(TValue value)
		{
			return ValueProvider<Unit>.FromConstantValue(value).ConvertSimple().ValueToString();
		}
	}
}
