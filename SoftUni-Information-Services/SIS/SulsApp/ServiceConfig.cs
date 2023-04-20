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
		}

		public void ConfigureServices()
		{
			var db = new ApplicationDbContext();
			db.Database.EnsureCreated();
		}
	}
}
