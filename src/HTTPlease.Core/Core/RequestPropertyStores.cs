namespace HTTPlease.Core
{
	/// <summary>
    /// 	Factories for <see cref="IRequestPropertyStore"/>.
    /// </summary>
	public static class RequestPropertyStores
	{
		/// <summary>
        /// 	Create a new mutable request property store.
        /// </summary>
		/// <typeparam name="TContext">
		/// 	The type of object used as a context for resolving deferred values.
		/// </typeparam>
        /// <returns>
		/// 	The new <see cref="IRequestPropertyStore"/>.
		/// </returns>
		public static IRequestPropertyStore Mutable<TContext>()
		{
			return new MutableRequestPropertyStore<TContext>();
		}

		/// <summary>
        /// 	Create a new immutable request property store.
        /// </summary>
        /// <typeparam name="TContext">
		/// 	The type of object used as a context for resolving deferred values.
		/// </typeparam>
		/// <returns>
		/// 	The new <see cref="IRequestPropertyStore"/>.
		/// </returns>
		public static IRequestPropertyStore Immutable<TContext>()
		{
			return ImmutableRequestPropertyStore<TContext>.Empty;
		}
	}
}