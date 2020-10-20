using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AsyncTests
{
	public class HttpClient
	{
		public void Post(string url, object payload)
		{
			var json = JsonConvert.SerializeObject(payload);
			var content = Encoding.ASCII.GetBytes(json);

			var request = (HttpWebRequest) WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "appliction/json";
			request.Accept = "appliction/json";
			request.ContentLength = content.Length;

			using (var stream = request.GetRequestStream())
			{
				stream.Write(content, 0, content.Length);
			}

			var response = (HttpWebResponse)request.GetResponse();
		}
	}
}
