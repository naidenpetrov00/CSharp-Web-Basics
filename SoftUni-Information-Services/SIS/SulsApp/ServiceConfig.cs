namespace SulsApp
{
	using Microsoft.EntityFrameworkCore;
	using SIS.HTTP;
	using SIS.MvcFramework;
	using System.Collections.Generic;

	public class ServiceConfig : IMvcApplication
	{
		public void Configure(IList<Route> routeTable)
		{
		}

		public void ConfigureServices()
		{
			var db = new ApplicationDbContext();
			db.Database.Migrate();
		}
	}
}
