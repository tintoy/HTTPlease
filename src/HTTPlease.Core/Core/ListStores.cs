namespace HTTPlease.Core
{
	/// <summary>
    /// 	Factories for <see cref="IListStore{TItem}"/>s.
    /// </summary>
	public static class ListPropertyStores
	{
		/// <summary>
        /// 	Create a new mutable list store.
        /// </summary>
        /// <typeparam name="TItem">
		/// 	The type of item contained in the list.
		/// </typeparam>
		/// <returns>
		/// 	The list store.
		/// </returns>
		public static IListStore<TItem> Mutable<TItem>() => new MutableListStore<TItem>();

		/// <summary>
        /// 	Create a new immutable list store.
        /// </summary>
        /// <typeparam name="TItem">
		/// 	The type of item contained in the list.
		/// </typeparam>
		/// <returns>
		/// 	The list store.
		/// </returns>
		public static IListStore<TItem> Immutable<TItem>() => new ImmutableListStore<TItem>();
	}
}