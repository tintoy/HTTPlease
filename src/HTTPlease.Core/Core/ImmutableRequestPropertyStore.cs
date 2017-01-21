using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace HTTPlease.Core
{
	/// <summary>
    /// 	A request property store that uses an <see cref="ImmutableDictionary{TKey, TValue}"/> as the backing store.
    /// </summary>
	sealed class ImmutableRequestPropertyStore
		: IRequestPropertyStore
	{
		/// <summary>
        /// 	The <see cref="ImmutableDictionary{TKey, TValue}"/> that acts as the backing store.
        /// </summary>
		readonly ImmutableDictionary<string, object> _properties;

		readonly object _syncRoot;

		/// <summary>
        /// 	Create a new <see cref="ImmutableRequestPropertyStore"/>.
        /// </summary>
		public ImmutableRequestPropertyStore()
			: this(properties: ImmutableDictionary<string, object>.Empty)
		{
		}

		/// <summary>
        /// 	Create a new <see cref="ImmutableRequestPropertyStore"/>.
        /// </summary>
        /// <param name="properties">
		/// 	The <see cref="ImmutableDictionary{TKey, TValue}"/> that acts as the backing store.
		/// </param>
		public ImmutableRequestPropertyStore(ImmutableDictionary<string, object> properties)
			: this(properties, syncRoot: new object())
		{
		}

		public ImmutableRequestPropertyStore(ImmutableDictionary<string, object> properties, object syncRoot)
		{
			if (properties == null)
				throw new ArgumentNullException(nameof(properties));

			if (syncRoot == null)
				throw new ArgumentNullException(nameof(syncRoot));

			_properties = properties;
			_syncRoot = syncRoot;
		}

		public bool Mutable => false;

        public object SyncRoot => _syncRoot;

		public bool ContainsKey(string name) => _properties.ContainsKey(name);

		public TValue Get<TValue>(string name) => (TValue)_properties[name];
        
        public bool TryGet<TValue>(string name, out TValue value)
        {
			if (name == null)
				throw new ArgumentNullException(nameof(name));

            object propertyValue;
			if (!_properties.TryGetValue(name, out propertyValue))
			{
				value = default(TValue);

				return false;
			}

			value = (TValue)propertyValue;

			return true;
        }

        public IRequestPropertyStore AddOrUpdate<TValue>(string name, TValue value)
        {
			if (name == null)
				throw new ArgumentNullException(nameof(name));

            object existingValue;
			if (_properties.TryGetValue(name, out existingValue) && Equals(existingValue, value))
				return this;

			return new ImmutableRequestPropertyStore(
				_properties.SetItem(name, value)
			);
        }

        public IRequestPropertyStore BatchEdit(Action<IDictionary<string, object>> editAction)
        {
			if (editAction == null)
				throw new ArgumentNullException(nameof(editAction));

            ImmutableDictionary<string, object>.Builder editor = _properties.ToBuilder();
			editAction(editor);

			return new ImmutableRequestPropertyStore(
				editor.ToImmutable()
			);
        }

		public IRequestPropertyStore Remove(string name)
        {
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			if (!_properties.ContainsKey(name))
				return this;

			return new ImmutableRequestPropertyStore(
				_properties.Remove(name)
			);
        }
    }
}