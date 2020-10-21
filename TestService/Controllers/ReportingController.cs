using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestService.Models;
using TestService.Utils;

namespace TestService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportingController : ControllerBase
	{
		private readonly ILogger<ReportingController> _logger;

		public ReportingController(ILogger<ReportingController> logger)
		{
			_logger = logger;
		}

		// GET: api/Reporting
		[HttpGet]
		public string Get() 
		{
			return "Reporting Controller is up";
		}

		// POST: api/Reporting
		[HttpPost]
		public void Post(ReportingRequest payload)
		{
			if (!ReportingStorage.Reporting.ContainsKey(payload.CalculationId))
			{
				ReportingStorage.Reporting.Add(payload.CalculationId, payload);
			}
			else 
			{
				_logger.LogWarning($"The test was already reported: {payload.CalculationId}");
			}
		}
	}
}
