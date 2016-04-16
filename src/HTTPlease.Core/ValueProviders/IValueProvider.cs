namespace HTTPlease.ValueProviders
{
	/// <summary>
	///		Represents the provider for a value from an instance of <typeparamref name="TContext"/>.
	/// </summary>
	/// <typeparam name="TContext">
	///		The source type from which the value is extracted.
	/// </typeparam>
	/// <typeparam name="TValue">
	///		The type of value returned by the provider.
	/// </typeparam>
	public interface IValueProvider<in TContext, out TValue>
		: ISimpleValueProvider<TValue>, IContextValueProvider<TContext, TValue>
	{
    }
}
