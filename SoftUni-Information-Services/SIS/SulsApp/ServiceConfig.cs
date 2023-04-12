namespace SulsApp
{
	using DemoApp;
	using SIS.HTTP;
	using SIS.MvcFramework;
	using SulsApp.Controllers;

	public class ServiceConfig : IMvcApplication
	{
		 
		public void Configure(IList<Route> routeTable)
		{
			routeTable.Add(new Route(HttpMethodType.GET, "/", new HomeController().Index));
			routeTable.Add(new Route(HttpMethodType.GET, "/Users/Login", new UsersController().Login));
			routeTable.Add(new Route(HttpMethodType.GET, "/Users/Register", new UsersController().Register));
		}

		public void ConfigureServices()
		{
			var db = new ApplicationDbContext();
			db.Database.EnsureCreated();
		}
	}
}
