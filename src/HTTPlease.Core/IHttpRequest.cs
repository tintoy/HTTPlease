using System;
using System.Collections.Generic;

namespace HTTPlease
{
	/// <summary>
	///		Represents a template for building HTTP requests.
	/// </summary>
	public interface IHttpRequest
    {
		/// <summary>
		///		The request URI.
		/// </summary>
		Uri RequestUri
		{
			get;
		}

		/// <summary>
		///		Is the request URI a template?
		/// </summary>
		bool IsUriTemplate
		{
			get;
		}

		/// <summary>
		///		Additional properties for the request.
		/// </summary>
		IReadOnlyDictionary<string, object> Properties
		{
			get;
		}
	}
}
