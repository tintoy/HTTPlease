namespace HTTPlease.Core
{
	/// <summary>
    /// 	Factories for <see cref="IListProperty{TItem}"/>s.
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
		public static IListProperty<TItem> Mutable<TItem>() => new MutableListProperty<TItem>();

		/// <summary>
        /// 	Create a new immutable list store.
        /// </summary>
        /// <typeparam name="TItem">
		/// 	The type of item contained in the list.
		/// </typeparam>
		/// <returns>
		/// 	The list store.
		/// </returns>
		public static IListProperty<TItem> Immutable<TItem>() => new ImmutableListProperty<TItem>();
	}
}