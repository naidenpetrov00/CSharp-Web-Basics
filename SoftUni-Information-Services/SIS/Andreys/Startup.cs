namespace Andreys
{
	using SIS.HTTP;
	using Andreys.Data;
	using SIS.MvcFramework;
	using System.Collections.Generic;
	using Andreys.Services.Interfaces;
	using Andreys.Services;

	public class Startup : IMvcApplication
	{
		public void Configure(IList<Route> routeTable)
		{
			var db = new ApplicationDbContext();
			db.Database.EnsureCreated();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.Add<IUserService, UserService>();
			services.Add<IProductService, ProductService>();
			services.Add<IHomeService, HomeService>();
		}
	}
}
