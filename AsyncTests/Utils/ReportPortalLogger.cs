using AsyncTests.Models;
using NUnit.Engine.Extensibility;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.NUnitExtension.EventArguments;
using System;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Shared.Reporter;

namespace AsyncTests
{
	[Extension]
	public class ReportPortalLogger : NUnit.Engine.ITestEventListener
	{
		public ReportPortalLogger()
		{
			ReportPortal.NUnitExtension.ReportPortalListener.BeforeTestFinished += ReportPortalListener_BeforeTestFinished;
		}

		public void OnTestEvent(string report) { }

		private void ReportPortalListener_BeforeTestFinished(object sender, TestItemFinishedEventArgs e)
		{
			var testInfo = GetTestInfo(e);

			if (testInfo.Name.Contains(CallbackIndicator))
			{
				if (e.FinishTestItemRequest.Status == Status.Skipped)
				{
					e.FinishTestItemRequest.Issue = new Issue
					{
						Type = WellKnownIssueType.NotDefect
					};
				};

				LogCalculationSerivceResponse(e.TestReporter, testInfo.Name);
				SendReportingRequest(testInfo);
			}

		}

		private void LogCalculationSerivceResponse(ITestReporter reporter, string testName)
		{
			if (!TestArtifactsStorage.Responses.TryGetValue(testName, out CalculationServiceResponse response))
			{
				return;
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("Data have been sent for processing to Calculation service.\n");
			sb.Append($"Server: {response.Server}\n");
			sb.Append($"Version: {response.Version}\n");
			sb.Append($"Calculation Id: {response.CalculationId}\n");
			sb.Append($"Request received: {response.RequestReceived}\n");

			reporter.Log(new CreateLogItemRequest
			{
				Level = LogLevel.Info,
				Time = DateTime.UtcNow,
				Text = sb.ToString()
			});
		}

		private void SendReportingRequest(TestItemResponse testInfo)
		{
			if (!TestArtifactsStorage.Reporting.TryGetValue(testInfo.Name, out ReportingRequest payload))
			{
				return;
			}

			payload.TestUuid = testInfo.Uuid;

			new HttpClient().Post(Configuration.ReportingUrl, payload);
		}

		private TestItemResponse GetTestInfo(TestItemFinishedEventArgs e)
		{
			// wait till the test is reported to the server and retrieve its info
			e.TestReporter.StartTask.Wait();
			var infoTask = Task.Run(async () => await e.Service.TestItem.GetAsync(e.TestReporter.Info.Uuid));
			infoTask.Wait();
			return infoTask.Result;
		}

		private const string CallbackIndicator = "_withCallback";
	}
}

