using AsyncTests.Models;
using System.Collections.Generic;

namespace AsyncTests
{
	public static class TestArtifactsStorage
	{
		public static Dictionary<string, ReportingRequest> Reporting = new Dictionary<string, ReportingRequest>();
		public static Dictionary<string, CalculationServiceResponse> Responses = new Dictionary<string, CalculationServiceResponse>();
	}
}
