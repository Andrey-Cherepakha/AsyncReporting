using System.Collections.Generic;
using TestService.Models;

namespace TestService.Utils
{
	public static class ReportingStorage
	{
		public static Dictionary<string, ReportingRequest> Reporting = new Dictionary<string, ReportingRequest>();
	}
}
