using AsyncTests.Models;
using NUnit.Framework;

namespace AsyncTests
{
	[TestFixture]
	public class Tests
	{
		[TestCase(2, 2, 4)]
		[TestCase(5, 5, 10)]
		public void ValidateRemoteCalculation_withCallback(int arg1, int arg2, int expected)
		{
			var payload = new CalculationServiceRequest();
			payload.Argument1 = arg1;
			payload.Argument2 = arg2;
			payload.CallbackUrl = Configuration.CallbackUrl;

			var service = new CalculationService();
			var response = service.Sum(payload);

			// create an instance of the reporting request and init some fields
			var reportingRequest = new ReportingRequest();
			reportingRequest.CalculationId = response.CalculationId;
			reportingRequest.Expected = expected;

			// store information in the internal sotrage for further usage in the Report Portal extension
			TestArtifactsStorage.Responses.Add(TestContext.CurrentContext.Test.Name, response);
			TestArtifactsStorage.Reporting.Add(TestContext.CurrentContext.Test.Name, reportingRequest);

			// mark the test as unfinished
			Assert.Inconclusive();
		}
	}
}
