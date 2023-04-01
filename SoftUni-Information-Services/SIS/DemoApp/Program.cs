namespace DemoApp
{
	using SIS.HTTP;
	using System.Text;
	using SIS.MvcFramework;
	using System;

	public class Program
	{
		static async Task Main(string[] args)
		{
			var routeTable = new List<Route>();
			routeTable.Add(new Route(HttpMethodType.GET, "/", Index));
			routeTable.Add(new Route(HttpMethodType.GET, "/users/login", Login));
			routeTable.Add(new Route(HttpMethodType.POST, "/users/login", DoLogin));
			routeTable.Add(new Route(HttpMethodType.GET, "/favicon.ico", FavIcon));

			var httpServer = new HttpServer(80, routeTable);
			await httpServer.StartAsync();
		}

		private static HttpResponse FavIcon(HttpRequest request)
		{
			throw new NotImplementedException();
		}

		public static HttpResponse Index(HttpRequest request)
		{
			var content = "<h1>Home page</h1>";
			var stringContent = Encoding.UTF8.GetBytes(content);
			var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
			response.Headers.Add(new Header("Content-type", "text/html"));

			return response;
		}

		public static HttpResponse Login(HttpRequest request)
		{
			var content = "<h1>Login page</h1>";
			var stringContent = Encoding.UTF8.GetBytes(content);
			var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
			response.Headers.Add(new Header("Content-type", "text/html"));

			return response;
		}
		public static HttpResponse DoLogin(HttpRequest request)
		{
			var content = "<h1>Login page</h1>";
			var stringContent = Encoding.UTF8.GetBytes(content);
			var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
			response.Headers.Add(new Header("Content-type", "text/html"));

			return response;
		}
	}
}