namespace HTTPlease.Core
{
	/// <summary>
    /// 	Represents a value store.
    /// </summary>
	public interface IStore
	{
		/// <summary>
        /// 	Is the value store mutable?
        /// </summary>
		bool Mutable { get; }

		/// <summary>
        /// 	An object that can be used to synchronise access to the store.
        /// </summary>
		/// <remarks>
        /// 	Mainly useful for mutable stores.
        /// </remarks>
		object SyncRoot { get; }
	}
}