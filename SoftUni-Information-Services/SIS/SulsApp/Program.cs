namespace SulsApp
{
	using DemoApp;
	using SIS.HTTP;
	using SulsApp.Controllers;

	public class Program
	{
		public static async Task Main()
		{
			var db = new ApplicationDbContext();
			db.Database.EnsureCreated();

			var routeTable = new List<Route>();
			routeTable.Add(new Route(HttpMethodType.GET, "/", new HomeController().Index));

			var httpServer = new HttpServer(80, routeTable);
			await httpServer.StartAsync();
		}
	}
}