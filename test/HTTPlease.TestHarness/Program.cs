using System;
using System.Diagnostics;
using System.Net.Http;

namespace HTTPlease.TestHarness
{
	/// <summary>
	/// Quick-and-dirty test harness for use when Visual Studio refuses to debug unit tests.
	/// </summary>
	static class Program
	{
		static void Main()
		{
			Trace.Listeners.Add(
				new TextWriterTraceListener(Console.Out)
			);

			try
			{
				string value1 = String.Empty;
				string value2 = String.Empty;

				HttpRequest request =
					HttpRequest.Factory.Create("http://localhost:1234/foo/bar")
						.WithQueryParameter("flag", () => value1)
						.WithQueryParameter("flag", () => value2);

				using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
				{
					Debug.WriteLine("URI = '{0}'", requestMessage.RequestUri);
				}

				value1 = null;

				using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
				{
					Debug.WriteLine("URI = '{0}'", requestMessage.RequestUri);
				}

				value2 = null;

				using (HttpRequestMessage requestMessage = request.BuildRequestMessage(HttpMethod.Get))
				{
					Debug.WriteLine("URI = '{0}'", requestMessage.RequestUri);
				}
			}
			catch (Exception unexpectedError)
			{
				Console.WriteLine(unexpectedError);
			}
		}
	}
}
