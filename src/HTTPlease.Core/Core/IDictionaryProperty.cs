using System;
using System.Collections.Generic;

namespace HTTPlease.Core
{
	/// <summary>
    /// 	Represents a mutable or immutable dictionary.
    /// </summary>
	/// <typeparam name="TKey">
	/// 	The type of key used to identify values in the dictionary.
	/// </typeparam>
	/// <typeparam name="TValue">
	/// 	The type of value contained in the dictionary.
	/// </typeparam>
	public interface IDictionaryProperty<TKey, TValue>
		: IStore, IReadOnlyDictionary<TKey, TValue>
	{
		/// <summary>
        /// 	Add a value to the dictionary.
        /// </summary>
		/// <param name="key">
		/// 	A key that uniquely identifies the value to add.
		/// </param>
        /// <param name="value">
		/// 	The value to add.
		/// </param>
        /// <returns>
		/// 	The updated dictionary property. May or may not be the same instance that <see cref="Add"/> was called on.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <c>null</c>.
		/// </exception>
		IDictionaryProperty<TKey, TValue> Add(TKey key, TValue value);

		/// <summary>
        /// 	Add or update a value in the dictionary.
        /// </summary>
        /// <param name="key">
		/// 	A key that uniquely identifies the value to add.
		/// </param>
        /// <param name="value">
		/// 	The value to add.
		/// </param>
        /// <returns>
		/// 	The updated dictionary property. May or may not be the same instance that <see cref="SetItem"/> was called on.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <c>null</c>.
		/// </exception>
		IDictionaryProperty<TKey, TValue> SetItem(TKey key, TValue value);

		/// <summary>
        /// 	Remove a value from the dictionary.
        /// </summary>
        /// <param name="key">
		/// 	A key that uniquely identifies the value to remove.
		/// </param>
        /// <returns>
		/// 	The updated dictionary property. May or may not be the same instance that <see cref="Remove"/> was called on.
		/// </returns>
		IDictionaryProperty<TKey, TValue> Remove(TKey key);

		/// <summary>
		/// 	Perform a batched edit of the dictionary.
		/// </summary>
		/// <param name="editAction">
		/// 	A delegate that performs the edit.
		/// </param>
		/// <returns>
		/// 	The updated dictionary property. May or may not be the same instance that <see cref="BatchEdit"/> was called on.
		/// </returns>
		IDictionaryProperty<TKey, TValue> BatchEdit(Action<IDictionary<TKey, TValue>> editAction);

		/// <summary>
        /// 	Remove all values from the dictionary.
        /// </summary>
        /// <returns>
		/// 	The updated dictionary property. May or may not be the same instance that <see cref="Clear"/> was called on.
		/// </returns>
		IDictionaryProperty<TKey, TValue> Clear();
	}
}
