namespace DemoApp
{
	using SIS.HTTP;
	using System.Text;

	public class HtmlResponse : HttpResponse
	{
		public HtmlResponse(string html)
			: base()
		{
			this.StatusCode = HttpResponseCode.Ok;
			var byteData = Encoding.UTF8.GetBytes(html);
			this.Body= byteData;
			this.Headers.Add(new Header("Content-type", "text/html"));
			this.Headers.Add(new Header("Content-Length", this.Body.Length.ToString()));
		}
	}
}
