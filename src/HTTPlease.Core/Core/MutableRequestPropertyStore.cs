using System;
using System.Collections.Generic;

namespace HTTPlease.Core
{
	using ValueProviders;

	/// <summary>
    /// 	A request property store that uses a <see cref="Dictionary{TKey, TValue}"/> as the backing store.
    /// </summary>
	sealed class MutableRequestPropertyStore
		: IRequestPropertyStore
	{
		/// <summary>
        /// 	Create the default properties for mutable <see cref="HttpRequest"/>s.
        /// </summary>
		public static Dictionary<string, object> CreateDefaultProperties() => new Dictionary<string, object>
		{
			[nameof(HttpRequest.RequestActions)] = new MutableListProperty<RequestAction<object>>(),
			[nameof(HttpRequest.ResponseActions)] = new MutableListProperty<ResponseAction<object>>(),
			[nameof(HttpRequest.TemplateParameters)] = new MutableDictionaryProperty<string, IValueProvider<object, string>>(),
			[nameof(HttpRequest.QueryParameters)] = new MutableDictionaryProperty<string, IValueProvider<object, string>>()
		};

		readonly object						_syncRoot = new object();

		/// <summary>
        /// 	The <see cref="Dictionary{TKey, TValue}"/> that acts as the backing store.
        /// </summary>
		readonly Dictionary<string, object>	_properties;

		/// <summary>
        /// 	Create a new <see cref="MutableRequestPropertyStore"/>.
        /// </summary>
		public MutableRequestPropertyStore()
			: this(properties: CreateDefaultProperties())
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

		/// <summary>
		/// 	Is the property store mutable?
		/// </summary>
		public bool Mutable => true;

		/// <summary>
		/// 	An object used to synchronise access to the property store.
		/// </summary>
        public object SyncRoot => _syncRoot;

		/// <summary>
		/// 	Determine whether the specified property is present in the store.
		/// </summary>
		/// <param name="name">
		/// 	The property name.
		/// </param>
		/// <returns>
		/// 	<c>true</c>, if the property is present; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsKey(string name) => _properties.ContainsKey(name);

		/// <summary>
		/// 	Get the value of the property with the specified name.
		/// </summary>
		/// <typeparam name="TValue">
		/// 	The type of value to retrieve.
		/// </typeparam>
		/// <param name="name">
		/// 	The property name.
		/// </param>
		public TValue Get<TValue>(string name) => (TValue)_properties[name];
        
		/// <summary>
		/// 	Get the value of the property with the specified name, if present.
		/// </summary>
		/// <typeparam name="TValue">
		/// 	The type of value to retrieve.
		/// </typeparam>
		/// <param name="name">
		/// 	The property name.
		/// </param>
		/// <param name="value">
		/// 	Receives the property value.
		/// </param>
		/// <returns>
		/// 	<c>true</c>, if the property is present; otherwise, <c>false</c>.
		/// </returns>
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

		/// <summary>
		/// 	Add or update the specified property.
		/// </summary>
		/// <param name="name">
		/// 	The property name.
		/// </param>
		/// <param name="value">
		/// 	The property value.
		/// </param>
		/// <returns>
		/// 	The request property store.
		/// </returns>
        public IRequestPropertyStore AddOrUpdate<TValue>(string name, TValue value)
        {
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			_properties[name] = value;
			
			return this;
        }

		/// <summary>
        /// 	Perform a batch edit of properties in the store.
        /// </summary>
        /// <param name="editAction">
		/// 	A delegate that receives an <see cref="IDictionary{TKey, TValue}"/> used to perform the edit.
		/// </param>
        /// <returns>
		/// 	The property store.
		/// </returns>
        public IRequestPropertyStore BatchEdit(Action<IDictionary<string, object>> editAction)
        {
			if (editAction == null)
				throw new ArgumentNullException(nameof(editAction));

            editAction(_properties);
			
			return this;
        }

		/// <summary>
		/// 	Remove a property from the store.
		/// </summary>
		/// <param name="name">
		/// 	The name of the property to remove.
		/// </param>
		/// <returns>
		/// 	The property store.
		/// </returns>
		public IRequestPropertyStore Remove(string name)
        {
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			_properties.Remove(name);

			return this;
        }
    }
}