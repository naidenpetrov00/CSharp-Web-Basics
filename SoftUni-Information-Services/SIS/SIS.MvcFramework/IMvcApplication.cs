using SIS.HTTP;

namespace SIS.MvcFramework
{
	public interface IMvcApplication
	{
		void Configure(IList<Route> routeTable);

		void ConfigureServices();
	}
}
