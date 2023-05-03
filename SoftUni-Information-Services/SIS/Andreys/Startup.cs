namespace Andreys
{
	using SIS.HTTP;
	using Andreys.Data;
	using SIS.MvcFramework;
	using System.Collections.Generic;

	public class Startup : IMvcApplication
	{
		public void Configure(IList<Route> routeTable)
		{
			var db = new ApplicationDbContext();
			db.Database.EnsureCreated();
		}

		public void ConfigureServices(IServiceCollection services)
		{

		}
	}
}
