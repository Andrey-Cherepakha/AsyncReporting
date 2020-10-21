using ReportPortal.Client;
using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Configuration;
using System;
using System.Text;
using TestService.Models;

namespace TestService.Utils
{
	public class ReportPortalUtil
	{
		public ReportPortalUtil()
		{
			var baseDir = AppDomain.CurrentDomain.BaseDirectory;
			var config = new ConfigurationBuilder().AddDefaults(baseDir).Build();

			var uri = config.GetValue<string>(ConfigurationPath.ServerUrl);
			var project = config.GetValue<string>(ConfigurationPath.ServerProject);
			var uuid = config.GetValue<string>(ConfigurationPath.ServerAuthenticationUuid);

			service = new Service(new Uri(uri), project, uuid);
		}

		private readonly Service service;

		public async void CompleteTest(string testUuid, bool isPassed)
		{
			var updateRequest = new UpdateTestItemRequest();
			updateRequest.Status = isPassed ? Status.Passed : Status.Failed;
			var testItem = await service.TestItem.GetAsync(testUuid);
			await service.TestItem.UpdateAsync(testItem.Id, updateRequest);
		}

		public async void LogCallback(string testUuid, CallbackRequest payload)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Data have been processed through Calculation service.\n");
			sb.Append($"Calculation id: {payload.CalculationId}\n");
			sb.Append($"Calculation started: {payload.CalculationStarted}\n");
			sb.Append($"Calculation finished: {payload.CalculationFinished}\n");
			sb.Append($"Result: {payload.Result}\n");

			await service.LogItem.CreateAsync(new CreateLogItemRequest
			{
				TestItemUuid = testUuid,
				Text = sb.ToString(),
				Time = DateTime.UtcNow,
				Level = LogLevel.Info
			});
		}
	}
}
