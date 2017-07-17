using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HTTPlease.Testability
{
	// TODO: Create abstraction for the various assertions common to all supported test frameworks.
	//		 The extension methods can then use these instead of framework-specific assertions.

	/// <summary>
	/// 	Extensible assertion functionality.
	/// </summary>
	public static class Assertions
	{
		/// <summary>
		/// 	Message-related assertions.
		/// </summary>
		public static IMessageAssertions Message { get; } = new MessageAssertions();
		
		/// <summary>
		/// 	Request-related assertions.
		/// </summary>
		public static IRequestAssertions Request { get; } = new RequestAssertions();

		/// <summary>
		/// 	Message-related assertion functionality.
		/// </summary>
		public interface IMessageAssertions
		{
		}

		/// <summary>
		/// 	Request-related assertion functionality.
		/// </summary>
		public interface IRequestAssertions
		{
		}

		class MessageAssertions
			: IMessageAssertions
		{
		}

		class RequestAssertions
			: IRequestAssertions
		{
		}
	}
}