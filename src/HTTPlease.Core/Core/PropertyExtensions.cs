using System;
using System.Collections.Generic;

namespace HTTPlease.Core
{
    using ValueProviders;

	/// <summary>
	/// 	Extension methods for working with common request properties.
	/// </summary>
	public static class PropertyExtensions
	{
		/// <summary>
		///		Get the specified request property, as a list property.
		/// </summary>
		/// <typeparam name="TItem">
		///		The type of item that the list contains.
		/// </typeparam>
		/// <param name="properties">
		///		The request properties.
		/// </param>
		/// <param name="propertyName">
		///		The name of the property to retrieve.
		/// </param>
		/// <returns>
		///		The property value.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="propertyName"/> is null, empty, or entirely composed of whitespace.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///		The specified property is not defined.
		/// </exception>
		public static IListProperty<TItem> GetList<TItem>(this IReadOnlyRequestPropertyStore properties, string propertyName)
		{
			return properties.Get<IListProperty<TItem>>(propertyName);
		}

		/// <summary>
		///		Get the specified request property (if defined), as a list property.
		/// </summary>
		/// <typeparam name="TItem">
		///		The type of item that the list contains.
		/// </typeparam>
		/// <param name="properties">
		///		The request properties.
		/// </param>
		/// <param name="list">
		/// 	Receives the list, if present.
		/// </param>
		/// <param name="propertyName">
		///		The name of the property to retrieve.
		/// </param>
		/// <returns>
		/// 	<c>true</c>, if the property is defined; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="propertyName"/> is null, empty, or entirely composed of whitespace.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///		The specified property is not defined.
		/// </exception>
		public static bool TryGetList<TItem>(this IReadOnlyRequestPropertyStore properties, string propertyName, out IListProperty<TItem> list)
		{
			return properties.TryGet<IListProperty<TItem>>(propertyName, out list);
		}

		/// <summary>
		///		Get the specified request property, as a dictionary property.
		/// </summary>
		/// <typeparam name="TKey">
		///		The type of key that uniquely identifies values in the dictionary.
		/// </typeparam>
		/// <typeparam name="TValue">
		///		The type of values contained in the dictionary.
		/// </typeparam>
		/// <param name="properties">
		///		The request properties.
		/// </param>
		/// <param name="propertyName">
		///		The name of the property to retrieve.
		/// </param>
		/// <returns>
		///		The property value.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="propertyName"/> is null, empty, or entirely composed of whitespace.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///		The specified property is not defined.
		/// </exception>
		public static IDictionaryProperty<TKey, TValue> GetDictionary<TKey, TValue>(this IReadOnlyRequestPropertyStore properties, string propertyName)
		{
			return properties.Get<IDictionaryProperty<TKey, TValue>>(propertyName);
		}

		/// <summary>
		///		Get the specified request property (if defined), as a dictionary property.
		/// </summary>
		/// <typeparam name="TKey">
		///		The type of key that uniquely identifies values in the dictionary.
		/// </typeparam>
		/// <typeparam name="TValue">
		///		The type of values contained in the dictionary.
		/// </typeparam>
		/// <param name="properties">
		///		The request properties.
		/// </param>
		/// <param name="propertyName">
		///		The name of the property to retrieve.
		/// </param>
		/// <param name="dictionary">
		/// 	Receives the dictionary, if present.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the property is defined; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="propertyName"/> is null, empty, or entirely composed of whitespace.
		/// </exception>
		/// <exception cref="KeyNotFoundException">
		///		The specified property is not defined.
		/// </exception>
		public static bool TryGetDictionary<TKey, TValue>(this IReadOnlyRequestPropertyStore properties, string propertyName, out IDictionaryProperty<TKey, TValue> dictionary)
		{
			return properties.TryGet<IDictionaryProperty<TKey, TValue>>(propertyName, out dictionary);
		}

		/// <summary>
		/// 	Remove multiple keys from the dictionary.
		/// </summary>
		/// <typeparam name="TKey">
		/// 	The type of key used to identify values in the dictionary.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// 	The type of value contained in the dictionary.
		/// </typeparam>
		/// <param name="dictionary">
		/// 	The dictionary.
		/// </param>
		/// <param name="keys">
		/// 	The keys to remove.
		/// </param>
		/// <returns>
		/// 	The updated dictionary.
		/// 
		/// 	May or may not be the same instance passed to RemoveRange.
		/// </returns>
		public static IDictionaryProperty<TKey, TValue> RemoveRange<TKey, TValue>(this IDictionaryProperty<TKey, TValue> dictionary, params TKey[] keys)
		{
			return dictionary.RemoveRange(
				(IEnumerable<TKey>)keys
			);
		}

		/// <summary>
		/// 	Remove multiple keys from the dictionary.
		/// </summary>
		/// <typeparam name="TKey">
		/// 	The type of key used to identify values in the dictionary.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// 	The type of value contained in the dictionary.
		/// </typeparam>
		/// <param name="dictionary">
		/// 	The dictionary.
		/// </param>
		/// <param name="keys">
		/// 	The keys to remove.
		/// </param>
		/// <returns>
		/// 	The updated dictionary.
		/// 
		/// 	May or may not be the same instance passed to RemoveRange.
		/// </returns>
		public static IDictionaryProperty<TKey, TValue> RemoveRange<TKey, TValue>(this IDictionaryProperty<TKey, TValue> dictionary, IEnumerable<TKey> keys)
		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			if (keys == null)
				throw new ArgumentNullException(nameof(keys));

			return dictionary.BatchEdit(editor =>
			{
				foreach (TKey key in keys)
					editor.Remove(key);
			});
		}

		/// <summary>
		/// 	Remove multiple items from the list.
		/// </summary>
		/// <typeparam name="TItem">
		/// 	The type of item contained in the list.
		/// </typeparam>
		/// <param name="list">
		/// 	The list.
		/// </param>
		/// <param name="items">
		/// 	The keys to remove.
		/// </param>
		/// <returns>
		/// 	The updated dictionary.
		/// 
		/// 	May or may not be the same instance passed to <see cref="RemoveRange{TItem}"/>.
		/// </returns>
		public static IListProperty<TItem> AddRange<TItem>(this IListProperty<TItem> list, IEnumerable<TItem> items)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			if (items == null)
				throw new ArgumentNullException(nameof(items));

			return list.BatchEdit(editor =>
			{
				foreach (TItem item in items)
					editor.Add(item);
			});
		}

		/// <summary>
		/// 	Remove multiple items from the list.
		/// </summary>
		/// <typeparam name="TItem">
		/// 	The type of item contained in the list.
		/// </typeparam>
		/// <param name="list">
		/// 	The list.
		/// </param>
		/// <param name="items">
		/// 	The keys to remove.
		/// </param>
		/// <returns>
		/// 	The updated dictionary.
		/// 
		/// 	May or may not be the same instance passed to <see cref="RemoveRange{TItem}"/>.
		/// </returns>
		public static IListProperty<TItem> RemoveRange<TItem>(this IListProperty<TItem> list, params TItem[] items)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));

			if (items == null)
				throw new ArgumentNullException(nameof(items));

			return list.BatchEdit(editor =>
			{
				foreach (TItem item in items)
					editor.Remove(item);
			});
		}

		/// <summary>
		/// 	Get the query parameter dictionary from request properties.
		/// </summary>
		/// <param name="properties">
		/// 	The request properties.
		/// </param>
		/// <returns>
		/// 	The request parameter dictionary.
		/// </returns>
		public static IDictionaryProperty<string, IValueProvider<object, string>> GetQueryParameters(this IReadOnlyRequestPropertyStore properties)
		{
			return properties.GetQueryParameters<object>();
		}

		/// <summary>
		/// 	Get the query parameter dictionary from request properties.
		/// </summary>
		/// <typeparam name="TContext">
		/// 	The type of object used as a context for resolving deferred values.
		/// </typeparam>
		/// <param name="properties">
		/// 	The request properties.
		/// </param>
		/// <returns>
		/// 	The request parameter dictionary.
		/// </returns>
		public static IDictionaryProperty<string, IValueProvider<TContext, string>> GetQueryParameters<TContext>(this IReadOnlyRequestPropertyStore properties)
		{
			return properties.Get<IDictionaryProperty<string, IValueProvider<TContext, string>>>(
				nameof(HttpRequest<TContext>.QueryParameters)
			);	
		}

		/// <summary>
		/// 	Get the query parameter dictionary from request properties.
		/// </summary>
		/// <param name="properties">
		/// 	The request properties.
		/// </param>
		/// <returns>
		/// 	The request parameter dictionary.
		/// </returns>
		public static IDictionaryProperty<string, IValueProvider<object, string>> GetTemplateParameters(this IReadOnlyRequestPropertyStore properties)
		{
			return properties.GetTemplateParameters<object>();
		}

		/// <summary>
		/// 	Get the query parameter dictionary from request properties.
		/// </summary>
		/// <typeparam name="TContext">
		/// 	The type of object used as a context for resolving deferred values.
		/// </typeparam>
		/// <param name="properties">
		/// 	The request properties.
		/// </param>
		/// <returns>
		/// 	The request parameter dictionary.
		/// </returns>
		public static IDictionaryProperty<string, IValueProvider<TContext, string>> GetTemplateParameters<TContext>(this IReadOnlyRequestPropertyStore properties)
		{
			return properties.Get<IDictionaryProperty<string, IValueProvider<TContext, string>>>(
				nameof(HttpRequest<TContext>.TemplateParameters)
			);	
		}
	}
}