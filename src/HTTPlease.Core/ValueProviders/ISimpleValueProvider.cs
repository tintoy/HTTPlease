namespace HTTPlease.ValueProviders
{
	/// <summary>
	///		Represents a provider for values that are not context-dependent.
	/// </summary>
	/// <typeparam name="TValue">
	///		The type of value returned by the provider.
	/// </typeparam>
    public interface ISimpleValueProvider<out TValue>
	{
		/// <summary>
		///		Get the provider's current value.
		/// </summary>
		/// <returns>
		///		The value.
		/// </returns>
		TValue Get();
	}
}
