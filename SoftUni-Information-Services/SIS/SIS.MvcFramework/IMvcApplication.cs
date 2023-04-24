namespace SIS.MvcFramework
{
	using SIS.HTTP;

	public interface IMvcApplication
	{
		void Configure(IList<Route> routeTable);

		void ConfigureServices(IServiceCollection services);
	}
}
