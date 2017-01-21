using System;
using System.Collections.Generic;

namespace HTTPlease.Core
{
	/// <summary>
    /// 	Represents a mutable or immutable list of items.
    /// </summary>
	/// <typeparam name="TItem">
	/// 	The type of item contained in the list.
	/// </typeparam>
	public interface IListStore<TItem>
		: IStore, IReadOnlyList<TItem>
	{
		/// <summary>
        /// 	Append an item to the list.
        /// </summary>
        /// <param name="item">
		/// 	The item to append.
		/// </param>
        /// <returns>
		/// 	The updated list store. May or may not be the same instance that <see cref="Add"/> was called on.
		/// </returns>
		IListStore<TItem> Add(TItem item);

		/// <summary>
        /// 	Insert an item into the list.
        /// </summary>
        /// <param name="index">
		/// 	The index that the item will be inserted at.
		/// </param>
		/// <param name="item">
		/// 	The item to insert.
		/// </param>
        /// <returns>
		/// 	The updated list store. May or may not be the same instance that <see cref="Insert"/> was called on.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="index"/> is greater than the number of items in the list.
		/// </exception>
		IListStore<TItem> Insert(int index,  TItem item);

		/// <summary>
        /// 	Remove an item from the list.
        /// </summary>
        /// <param name="item">
		/// 	The item to remove (only the first occurrence is remove).
		/// </param>
        /// <returns>
		/// 	The updated list store. May or may not be the same instance that <see cref="Remove"/> was called on.
		/// </returns>
		IListStore<TItem> Remove(TItem item);

		/// <summary>
        /// 	Remove the item at the specified index.
        /// </summary>
        /// <param name="index">
		/// 	The index of the item to be removed.
		/// </param>
        /// <returns>
		/// 	The updated list store. May or may not be the same instance that <see cref="RemoveAt"/> was called on.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="index"/> is greater than or equal to the number of items in the list.
		/// </exception>
		IListStore<TItem> RemoveAt(int index);

		/// <summary>
        /// 	Remove all items from the list.
        /// </summary>
        /// <returns>
		/// 	The updated list store. May or may not be the same instance that <see cref="Clear"/> was called on.
		/// </returns>
		IListStore<TItem> Clear();
	}
}
