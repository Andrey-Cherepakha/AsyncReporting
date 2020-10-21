using System;

namespace TestService.Models
{
	public class CallbackRequest
	{
		public string CalculationId { get; set; }
		public DateTime CalculationStarted { get; set; }
		public DateTime CalculationFinished { get; set; }
		public int Result { get; set; }
	}
}
