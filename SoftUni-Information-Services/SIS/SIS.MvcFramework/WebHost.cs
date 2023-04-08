namespace SIS.MvcFramework
{
	using SIS.HTTP;

	public class WebHost
	{
		public static async Task Start(IMvcApplication app)
		{

			var routeTable = new List<Route>();
			app.ConfigureServices();
			app.Configure(routeTable);
			var httpServer = new HttpServer(80, routeTable);
			await httpServer.StartAsync();
		}
	}
}
