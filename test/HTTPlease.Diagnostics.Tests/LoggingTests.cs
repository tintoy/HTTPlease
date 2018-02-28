using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HTTPlease.Diagnostics.Tests
{
	using Testability;
	using Testability.Mocks;

	using EventIds = Diagnostics.MessageHandlers.LoggerExtensions.LogEventIds;

	/// <summary>
	///		Tests for the HTTPlease.Diagnostics logging facility.
	/// </summary>
	public sealed class LoggingTests
    {
		/// <summary>
		///		Create a new logging test suite.
		/// </summary>
		public LoggingTests()
		{
		}

		/// <summary>
		///		Verify that BeginRequest / EndRequest log entries are emitted for a successful HTTP GET request.
		/// </summary>
		[Fact(DisplayName = "Emit BeginRequest / EndRequest log entries for successful HTTP GET")]
		public async Task Get_Request_Emits_LogEntries()
		{
			var logEntries = new List<LogEntry>();

			TestLogger logger = new TestLogger(LogLevel.Information);
			logger.LogEntries.Subscribe(
				logEntry => logEntries.Add(logEntry)
			);

			ClientBuilder clientBuilder = new ClientBuilder()
				.WithLogging(logger);

			HttpClient client = clientBuilder.CreateClient("http://localhost:1234", new MockMessageHandler(
				request => request.CreateResponse(HttpStatusCode.OK)
			));
			using (client)
			using (HttpResponseMessage response = await client.GetAsync("/test"))
			{
				response.EnsureSuccessStatusCode();
			}

			Assert.Equal(2, logEntries.Count);

			LogEntry logEntry1 = logEntries[0];
			Assert.Equal(EventIds.BeginRequest, logEntry1.EventId.Id);
			Assert.Equal("Performing GET request to 'http://localhost:1234/test'.",
				logEntry1.Message
			);
			Assert.Equal("GET",
				logEntry1.Properties["Method"]
			);
			Assert.Equal(new Uri("http://localhost:1234/test"),
				logEntry1.Properties["RequestUri"]
			);

			LogEntry logEntry2 = logEntries[1];
			Assert.Equal(EventIds.EndRequest, logEntry2.EventId.Id);
			Assert.Equal("Completed GET request to 'http://localhost:1234/test' (OK).",
				logEntry2.Message
			);
			Assert.Equal("GET",
				logEntry2.Properties["Method"]
			);
			Assert.Equal(new Uri("http://localhost:1234/test"),
				logEntry2.Properties["RequestUri"]
			);
			Assert.Equal(HttpStatusCode.OK,
				logEntry2.Properties["StatusCode"]
			);
		}

		/// <summary>
		///		Verify that <see cref="TestLogger"/> can emit an informational log entry.
		/// </summary>
		[Fact(DisplayName = "TestLogger.LogInformation succeeds")]
		public void TestLogger_LogInformation_Success()
		{
			const string name = "World";
			const LogLevel expectedLogLevel = LogLevel.Information;

			var logEntries = new List<LogEntry>();

			TestLogger logger = new TestLogger(LogLevel.Information);
			logger.LogEntries.Subscribe(
				logEntry => logEntries.Add(logEntry)
			);

			logger.LogInformation("Hello, {Name}!", name);

			Assert.Single(logEntries, logEntry =>
			{
				Assert.Equal(0, logEntry.EventId.Id);
				Assert.Null(logEntry.EventId.Name);

				Assert.Equal(expectedLogLevel, logEntry.Level);

				Assert.Equal($"Hello, {name}!", logEntry.Message);
				Assert.True(
					logEntry.Properties.ContainsKey("Name")
				);
				Assert.Equal(name, logEntry.Properties["Name"]);

				return true;
			});
		}
    }
}
