using System.Collections.Generic;

namespace HTTPlease.Core
{
	/// <summary>
    /// 	A read-only abstraction of the mutable or immutable backing store for <see cref="IHttpRequest"/> / <see cref="IHttpRequest{TContext}"/> properties.
    /// </summary>
	public interface IReadOnlyRequestPropertyStore
		: IStore
	{
		/// <summary>
		/// 	The names of properties that are defined in the store.
		/// </summary>
		IEnumerable<string> DefinedProperties { get; }

		/// <summary>
        /// 	Determine whether the store contains the specified property.
        /// </summary>
        /// <param name="name">
		/// 	The property name.
		/// </param>
        /// <returns>
		/// 	<c>true</c>, if the store contains the property; otherwise, <c>false</c>.
		/// </returns>
		bool ContainsKey(string name);

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
	}
}