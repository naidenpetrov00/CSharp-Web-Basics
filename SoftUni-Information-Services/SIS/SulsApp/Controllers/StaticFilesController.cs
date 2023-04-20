namespace SulsApp.Controllers
{
	using SIS.HTTP;
	using SIS.HTTP.Response;
	using SIS.MvcFramework;
	using System;

	public class StaticFilesController : Controller
	{
		public HttpResponse Site(HttpRequest request)
		{
			return new FileResponse(File.ReadAllBytes("wwwroot/css/site.css"), "text/css");
		}
	}
}
