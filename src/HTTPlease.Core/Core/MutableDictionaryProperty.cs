using System;
using System.Collections;
using System.Collections.Generic;

namespace HTTPlease.Core
{
	/// <summary>
	/// 	A dictionary that uses <see cref="Dictionary{TKey, TValue}"/> as the backing store.
	/// </summary>
	sealed class MutableDictionaryProperty<TKey, TValue>
		: IDictionaryProperty<TKey, TValue>
	{
		/// <summary>
		/// 	The underlying <see cref="Dictionary{TKey, TValue}"/>.
		/// </summary>
		readonly Dictionary<TKey, TValue> _dictionary;

		/// <summary>
		/// 	Create a new <see cref="MutableDictionaryProperty{TKey, TValue}"/>.
		/// </summary>
		public MutableDictionaryProperty()
			: this(dictionary: new Dictionary<TKey, TValue>())
		{
		}

		/// <summary>
		/// 	Create a new <see cref="MutableDictionaryProperty{TKey, TValue}"/>.
		/// </summary>
		/// <param name="dictionary">
		/// 	The <see cref="Dictionary{TKey, TValue}"/> that will act as the backing store.
		/// </param>
		public MutableDictionaryProperty(Dictionary<TKey, TValue> dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			_dictionary = dictionary;
		}

		/// <summary>
		/// 	Is the collection mutable?
		/// </summary>
		public bool Mutable => true;

		/// <summary>
		/// 	An object used to synchronise access to the dictionary.
		/// </summary>
        public object SyncRoot => _dictionary;

		/// <summary>
		/// 	Get the value with the specified key.
		/// </summary>
        /// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <c>null</c>.
		/// </exception>
		public TValue this[TKey key] => _dictionary[key];

		/// <summary>
		/// 	The number of values in the dictionary.
		/// </summary>
        public int Count => _dictionary.Count;

		/// <summary>
		///		A sequence containing all the keys in the dictionary.
		/// </summary>
        public IEnumerable<TKey> Keys => _dictionary.Keys;

		/// <summary>
		/// 	A sequence containing all the values in the dictionary.
		/// </summary>
        public IEnumerable<TValue> Values => _dictionary.Values;

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
		/// 	The updated dictionary property.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <c>null</c>.
		/// </exception>
        public IDictionaryProperty<TKey, TValue> Add(TKey key, TValue value)
        {
			_dictionary.Add(key, value);
			
			return this;
        }

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
		/// 	The updated dictionary property.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <c>null</c>.
		/// </exception>
		public IDictionaryProperty<TKey, TValue> Set(TKey key, TValue value)
        {
			_dictionary[key] = value;
			
			return this;
        }

		/// <summary>
        /// 	Remove a value from the dictionary.
        /// </summary>
        /// <param name="key">
		/// 	A key that uniquely identifies the value to remove.
		/// </param>
        /// <returns>
		/// 	The updated dictionary property.
		/// </returns>
		public IDictionaryProperty<TKey, TValue> Remove(TKey key)
        {
            _dictionary.Remove(key);
			
			return this;
        }

		/// <summary>
		/// 	Perform a batched edit of the dictionary.
		/// </summary>
		/// <param name="editAction">
		/// 	A delegate that performs the edit.
		/// </param>
		/// <returns>
		/// 	The updated dictionary property.
		/// </returns>
        public IDictionaryProperty<TKey, TValue> BatchEdit(Action<IDictionary<TKey, TValue>> editAction)
        {
			if (editAction == null)
				throw new ArgumentNullException(nameof(editAction));

			lock(SyncRoot)
			{
				editAction(_dictionary);
			}
            
			return this;
        }

		/// <summary>
        /// 	Remove all values from the dictionary.
        /// </summary>
        /// <returns>
		/// 	The updated dictionary property.
		/// </returns>
		public IDictionaryProperty<TKey, TValue> Clear()
		{
			_dictionary.Clear();

			return this;
		}

		/// <summary>
		/// 	Determine whether the dictionary contains the specified key.
		/// </summary>
		/// <param name="key">
		/// 	The key.
		/// </param>
		/// <returns>
		/// 	<c>true</c>, if the dictionary contains the key; otherwise, <c>false</c>.
		/// </returns>
        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

		/// <summary>
		/// 	Retrieve a value from the dictionary (if present).
		/// </summary>
		/// <param name="key">
		/// 	A key that uniquely identifies the value to retrieve.
		/// </param>
		/// <param name="value">
		/// 	Receives the value, if it is present.
		/// </param>
		/// <returns>
		/// 	<c>true</c>, if the dictionary contains the key; otherwise, <c>false</c>.
		/// </returns>
		public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

		/// <summary>
		/// 	Get a typed enumerator for items in the dictionary.
		/// </summary>
		/// <returns>
		/// 	The enumerator.
		/// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

		/// <summary>
		/// 	Get an untyped enumerator for items in the dictionary.
		/// </summary>
		/// <returns>
		/// 	The enumerator.
		/// </returns>
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
    }
}