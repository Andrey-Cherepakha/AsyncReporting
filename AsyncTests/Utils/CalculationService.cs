using AsyncTests.Models;
using System;

namespace AsyncTests
{
	public class CalculationService
	{
		public CalculationServiceResponse Sum(CalculationServiceRequest request)
		{
			// let the caller know that request is accepted

			var response = new CalculationServiceResponse();
			response.Server = "calculation.host.com";
			response.Version = "1.0";
			response.CalculationId = Guid.NewGuid().ToString();
			response.RequestReceived = DateTime.Now;

			return response;

			// Perform calculations.
			// Once the calculations are done, call Callback URL to pass the results.
		}
	}
}
