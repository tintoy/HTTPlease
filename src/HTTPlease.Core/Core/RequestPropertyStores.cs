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
        /// <returns>
		/// 	The new <see cref="IRequestPropertyStore"/>.
		/// </returns>
		public static IRequestPropertyStore Mutable()
		{
			return new MutableRequestPropertyStore();
		}

		/// <summary>
        /// 	Create a new immutable request property store.
        /// </summary>
        /// <returns>
		/// 	The new <see cref="IRequestPropertyStore"/>.
		/// </returns>
		public static IRequestPropertyStore Immutable()
		{
			return new ImmutableRequestPropertyStore();
		}
	}
}