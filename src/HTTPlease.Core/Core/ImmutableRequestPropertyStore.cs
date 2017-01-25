using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace HTTPlease.Core
{
	using ValueProviders;

	/// <summary>
    /// 	A request property store that uses an <see cref="ImmutableDictionary{TKey, TValue}"/> as the backing store.
    /// </summary>
	/// <typeparam name="TContext">
	/// 	The type of object used as a context for resolving deferred values.
	/// </typeparam>
	sealed class ImmutableRequestPropertyStore<TContext>
		: IRequestPropertyStore
	{
		/// <summary>
        /// 	The default properties for immutable <see cref="HttpRequest"/>s.
        /// </summary>
		public static readonly ImmutableDictionary<string, object> DefaultProperties =
			new Dictionary<string, object>
			{
				[nameof(HttpRequest.RequestActions)] = ImmutableListProperty<RequestAction<TContext>>.Empty,
				[nameof(HttpRequest.ResponseActions)] = ImmutableListProperty<ResponseAction<TContext>>.Empty,
				[nameof(HttpRequest.TemplateParameters)] = ImmutableDictionaryProperty<string, IValueProvider<TContext, string>>.Empty,
				[nameof(HttpRequest.QueryParameters)] = ImmutableDictionaryProperty<string, IValueProvider<TContext, string>>.Empty
			}
			.ToImmutableDictionary();

		/// <summary>
		/// 	The empty request property store.
		/// </summary>
		public static readonly ImmutableRequestPropertyStore<TContext> Empty = new ImmutableRequestPropertyStore<TContext>(DefaultProperties);

		/// <summary>
        /// 	The <see cref="ImmutableDictionary{TKey, TValue}"/> that acts as the backing store.
        /// </summary>
		readonly ImmutableDictionary<string, object> _properties;

		/// <summary>
		/// 	An object used to synchronise access to the property store.
		/// </summary>
		readonly object _syncRoot;

		/// <summary>
        /// 	Create a new <see cref="ImmutableRequestPropertyStore{TContext}"/>.
        /// </summary>
        /// <param name="properties">
		/// 	The <see cref="ImmutableDictionary{TKey, TValue}"/> that acts as the backing store.
		/// </param>
		public ImmutableRequestPropertyStore(ImmutableDictionary<string, object> properties)
			: this(properties, syncRoot: new object())
		{
		}

		/// <summary>
        /// 	Create a new <see cref="ImmutableRequestPropertyStore{TContext}"/>.
        /// </summary>
        /// <param name="properties">
		/// 	The <see cref="ImmutableDictionary{TKey, TValue}"/> that acts as the backing store.
		/// </param>
		/// <param name="syncRoot">
		/// 	An object used to synchronise access to the property store.
		/// </param>
		public ImmutableRequestPropertyStore(ImmutableDictionary<string, object> properties, object syncRoot)
		{
			if (properties == null)
				throw new ArgumentNullException(nameof(properties));

			if (syncRoot == null)
				throw new ArgumentNullException(nameof(syncRoot));

			_properties = properties;
			_syncRoot = syncRoot;
		}

		/// <summary>
		/// 	Is the property store mutable?
		/// </summary>
		public bool Mutable => false;

		/// <summary>
		/// 	An object used to synchronise access to the property store.
		/// </summary>
        public object SyncRoot => _syncRoot;

		/// <summary>
		/// 	The names of properties that are defined in the store.
		/// </summary>
		public IEnumerable<string> DefinedProperties => _properties.Keys;

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

            object existingValue;
			if (_properties.TryGetValue(name, out existingValue) && Equals(existingValue, value))
				return this;

			return new ImmutableRequestPropertyStore<TContext>(
				_properties.SetItem(name, value)
			);
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

            ImmutableDictionary<string, object>.Builder editor = _properties.ToBuilder();
			editAction(editor);

			return new ImmutableRequestPropertyStore<TContext>(
				editor.ToImmutable()
			);
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

			if (!_properties.ContainsKey(name))
				return this;

			return new ImmutableRequestPropertyStore<TContext>(
				_properties.Remove(name)
			);
        }
    }
}