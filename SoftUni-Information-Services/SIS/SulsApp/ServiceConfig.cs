namespace SulsApp
{
	using SIS.HTTP;
	using SIS.HTTP.Logging;
	using SIS.MvcFramework;
	using SulsApp.Services;
	using System.Collections.Generic;
	using Microsoft.EntityFrameworkCore;

	public class ServiceConfig : IMvcApplication
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.Add<IUsersService, UsersService>();
			services.Add<ILogger, MyLogger>();
		}

		public void Configure(IList<Route> routeTable)
		{
			var db = new ApplicationDbContext();
			db.Database.Migrate();
		}
	}
}