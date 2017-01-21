using System;
using System.Collections;
using System.Collections.Generic;

namespace HTTPlease.Core
{
	/// <summary>
    /// 	A mutable list that uses a <see cref="List{TItem}"/> as the backing store.
    /// </summary>
	/// <typeparam name="TItem">
	/// 	The type of item contained in the list.
	/// </typeparam>
	sealed class MutableListStore<TItem>
		: IListStore<TItem>
	{
		/// <summary>
        ///		The <see cref="List{TItem}"/> used as a backing store.
        /// </summary>
		readonly List<TItem> _list;

		/// <summary>
        ///		Create a new <see cref="MutableListStore{TItem}"/>.
        /// </summary>
		public MutableListStore()
			: this(list: new List<TItem>())
		{
		}

		/// <summary>
        ///		Create a new <see cref="MutableListStore{TItem}"/>.
        /// </summary>
        /// <param name="list">
		/// 	The <see cref="List{TItem}"/> to use as a backing store.
		/// </param>
		public MutableListStore(List<TItem> list)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			_list = list;
		}

		/// <summary>
        /// 	Get the item at the specified index.
        /// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is greater than or equal to the number of items in the list.
		/// </exception>
        public TItem this[int index] => _list[index];

        /// <summary>
        ///		The number of items in the list.
        /// </summary>
        public int Count => _list.Count;

		/// <summary>
        ///		Is the collection mutable?
        /// </summary>
        public bool Mutable => true;

		/// <summary>
        ///		An object used to synchronise access to the list.
        /// </summary>
        public object SyncRoot => _list;

		/// <summary>
        /// 	Append an item to the list.
        /// </summary>
        /// <param name="item">
		/// 	The item to append.
		/// </param>
        /// <returns>
		/// 	The updated list store.
		/// </returns>
        public IListStore<TItem> Add(TItem item)
        {
            _list.Add(item);

			return this;
        }

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
		/// 	The updated list store.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="index"/> is greater than the number of items in the list.
		/// </exception>
        public IListStore<TItem> Insert(int index, TItem item)
        {
			_list.Insert(index, item);

            return this;
        }

		/// <summary>
        /// 	Remove an item from the list.
        /// </summary>
        /// <param name="item">
		/// 	The item to remove (only the first occurrence is remove).
		/// </param>
        /// <returns>
		/// 	The updated list store.
		/// </returns>
        public IListStore<TItem> Remove(TItem item)
        {
            _list.Remove(item);

			return this;
        }

		/// <summary>
        /// 	Remove the item at the specified index.
        /// </summary>
        /// <param name="index">
		/// 	The index of the item to be removed.
		/// </param>
        /// <returns>
		/// 	The updated list store.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="index"/> is greater than or equal to the number of items in the list.
		/// </exception>
        public IListStore<TItem> RemoveAt(int index)
        {
            _list.RemoveAt(index);

			return this;
        }

		/// <summary>
        /// 	Remove all items from the list.
        /// </summary>
        /// <returns>
		/// 	The updated list store.
		/// </returns>
		public IListStore<TItem> Clear()
        {
            _list.Clear();

			return this;
        }

		/// <summary>
        /// 	Get a typed enumerator for the items in the list.
        /// </summary>
        /// <returns>
		/// 	The typed enumerator.
		/// </returns>
		public IEnumerator<TItem> GetEnumerator() => _list.GetEnumerator();

		/// <summary>
        /// 	Get an untyped enumerator for the items in the list.
        /// </summary>
        /// <returns>
		/// 	The untyped enumerator.
		/// </returns>
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
    }
}