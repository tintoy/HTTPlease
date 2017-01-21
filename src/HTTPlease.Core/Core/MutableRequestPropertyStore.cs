using System;
using System.Collections.Generic;

namespace HTTPlease.Core
{
	/// <summary>
    /// 	A request property store that uses a <see cref="Dictionary{TKey, TValue}"/> as the backing store.
    /// </summary>
	sealed class MutableRequestPropertyStore
		: IRequestPropertyStore
	{
		readonly object						_syncRoot = new object();

		/// <summary>
        /// 	The <see cref="Dictionary{TKey, TValue}"/> that acts as the backing store.
        /// </summary>
		readonly Dictionary<string, object>	_properties;

		/// <summary>
        /// 	Create a new <see cref="MutableRequestPropertyStore"/>.
        /// </summary>
		public MutableRequestPropertyStore()
			: this(properties: new Dictionary<string, object>())
		{
		}

		/// <summary>
        /// 	Create a new <see cref="MutableRequestPropertyStore"/>.
        /// </summary>
        /// <param name="properties">
		/// 	The <see cref="Dictionary{TKey, TValue}"/> that acts as the backing store.
		/// </param>
		public MutableRequestPropertyStore(Dictionary<string, object> properties)
		{
			if (properties == null)
				throw new ArgumentNullException(nameof(properties));

			_properties = properties;
		}

		public bool Mutable => true;

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

			_properties[name] = value;
			
			return this;
        }

        public IRequestPropertyStore BatchEdit(Action<IDictionary<string, object>> editAction)
        {
			if (editAction == null)
				throw new ArgumentNullException(nameof(editAction));

            editAction(_properties);
			
			return this;
        }

		public IRequestPropertyStore Remove(string name)
        {
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			_properties.Remove(name);

			return this;
        }
    }
}