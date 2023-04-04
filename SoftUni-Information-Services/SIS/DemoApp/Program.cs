namespace DemoApp
{
	using SIS.HTTP;
	using SIS.HTTP.Response;

	public class Program
	{
		public static async Task Main(string[] args)
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
			var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
			return new FileResponse(byteContent, "image/x-icon");
		}

		public static HttpResponse Index(HttpRequest request)
		{
			return new HtmlResponse("<h1>Home page</h1>");
		}

		public static HttpResponse Login(HttpRequest request)
		{
			return new HtmlResponse("<h1>Login page</h1>");
		}
		public static HttpResponse DoLogin(HttpRequest request)
		{
			return new HtmlResponse("<h1>Login page</h1>");
		}
	}
}