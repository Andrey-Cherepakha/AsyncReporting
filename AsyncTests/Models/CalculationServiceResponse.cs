using System;

namespace AsyncTests.Models
{
	public class CalculationServiceResponse
	{
		public string Server { get; set; }
		public string Version { get; set; }
		public string CalculationId { get; set; }
		public DateTime RequestReceived { get; set; }
	}
}
