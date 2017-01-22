namespace HTTPlease.Core
{
	/// <summary>
    /// 	Factories for <see cref="IDictionaryProperty{TKey, TValue}"/>s.
    /// </summary>
	public static class DictionaryPropertyStores
	{
		/// <summary>
        /// 	Create a new mutable dictionary store.
        /// </summary>
        /// <typeparam name="TKey">
		/// 	The type of key used to identify values in the dictionary.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// 	The type of value stored in the dictionary.
		/// </typeparam>
		/// <returns>
		/// 	The dictionary store.
		/// </returns>
		public static IDictionaryProperty<TKey, TValue> Mutable<TKey, TValue>() => new MutableDictionaryProperty<TKey, TValue>();

		/// <summary>
        /// 	Create a new immutable dictionary store.
        /// </summary>
        /// <typeparam name="TKey">
		/// 	The type of key used to identify values in the dictionary.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// 	The type of value stored in the dictionary.
		/// </typeparam>
		/// <returns>
		/// 	The dictionary store.
		/// </returns>
		public static IDictionaryProperty<TKey, TValue> Immutable<TKey, TValue>() => ImmutableDictionaryProperty<TKey, TValue>.Empty;
	}
}