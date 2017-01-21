using System;
using System.Collections.Generic;

namespace HTTPlease.Core
{
	/// <summary>
    /// 	An abstraction of the mutable or immutable backing store for <see cref="IHttpRequest"/> / <see cref="IHttpRequest{TContext}"/> properties.
    /// </summary>
	public interface IRequestPropertyStore
	{
		/// <summary>
        /// 	An object that can be used to synchronise access to the property store.
        /// </summary>
		/// <remarks>
        /// 	Mainly useful for mutable property stores.
        /// </remarks>
		object SyncRoot { get; }

		/// <summary>
        /// 	Determine whether the store contains the specified property.
        /// </summary>
        /// <param name="name">
		/// 	The property name.
		/// </param>
        /// <returns>
		/// 	<c>true</c>, if the store contains the property; otherwise, <c>false</c>.
		/// </returns>
		bool Contains(string name);

		/// <summary>
        /// 	Get the value of the specified property.
        /// </summary>
		/// <typeparam name="TValue">
		/// 	The type of property value to retrieve.
		/// </typeparam>
        /// <param name="name">
		/// 	The property name.
		/// </param>
        /// <returns>
		/// 	The property value.
		/// </returns>
		TValue Get<TValue>(string name);

		/// <summary>
        /// 	Get the value of the specified property.
        /// </summary>
		/// <typeparam name="TValue">
		/// 	The type of property value to retrieve.
		/// </typeparam>
        /// <param name="name">
		/// 	The property name.
		/// </param>
		/// <param name="value">
		/// 	Receives the property value.
		/// </param>
        /// <returns>
		/// 	<c>true</c>, if the store contains the property; otherwise, <c>false</c>.
		/// </returns>
		bool TryGet<TValue>(string name, out TValue value);

		/// <summary>
        /// 	Add or update a property in the store.
        /// </summary>
		/// <typeparam name="TValue">
		/// 	The type of property value to add or update.
		/// </typeparam>
        /// <param name="name">
		/// 	The property name.
		/// </param>
        /// <param name="value">
		/// 	The property value.
		/// </param>
        /// <returns>
		/// 	The property store.
		/// 
		///		This may or may not be the same instance that <see cref="AddOrUpdate"/> was called on.
		/// </returns>
		IRequestPropertyStore AddOrUpdate<TValue>(string name, TValue value);

		/// <summary>
        /// 	Remove a property from the store.
        /// </summary>
        /// <param name="name">
		/// 	The property name.
		/// </param>
        /// <returns>
		/// 	The property store.
		/// 
		///		This may or may not be the same instance that <see cref="Remove"/> was called on.
		/// </returns>
		IRequestPropertyStore Remove(string name);

		/// <summary>
        /// 	Perform a batch edit of properties in the store.
        /// </summary>
        /// <param name="editAction">
		/// 	A delegate that receives an <see cref="IDictionary{TKey, TValue}"/> used to perform the edit.
		/// </param>
        /// <returns>
		/// 	The property store.
		/// 
		///		This may or may not be the same instance that <see cref="BatchEdit"/> was called on.
		/// </returns>
		IRequestPropertyStore BatchEdit(Action<IDictionary<string, object>> editAction);
	}
}