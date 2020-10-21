using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestService.Models;
using TestService.Utils;

namespace TestService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CallbackController : ControllerBase
	{
		private readonly ILogger<CallbackController> _logger;
		private readonly ReportPortalUtil _reportPortal;

		public CallbackController(ILogger<CallbackController> logger)
		{
			_logger = logger;
			_reportPortal = new ReportPortalUtil();
		}

		// GET: api/Callback
		[HttpGet]
		public string Get() 
		{
			return "Callback Controller is up";
		}

		// POST: api/Callback
		[HttpPost]
		public void Post(CallbackRequest payload)
		{
			if (!ReportingStorage.Reporting.TryGetValue(payload.CalculationId, out ReportingRequest reporting))
			{
				_logger.LogError($"The test is absent: {payload.CalculationId}");
				return;
			}

			// Do some validation here
			var result = payload.Result == reporting.Expected;

			// Complete the test on Report Portal
			_reportPortal.LogCallback(reporting.TestUuid, payload);
			_reportPortal.CompleteTest(reporting.TestUuid, result);
			ReportingStorage.Reporting.Remove(payload.CalculationId);
		}
	}
}
