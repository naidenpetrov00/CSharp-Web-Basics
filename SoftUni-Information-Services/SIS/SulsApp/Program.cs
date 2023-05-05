namespace SulsApp
{
	using SIS.MvcFramework;

	public class Program
	{
		public static async Task Main()
		{
			await WebHost.StartAsync(new ServiceConfig());

			//var builder = WebApplication.CreateBuilder(args);
			//ConfigureServices(builder.Services, builder.Configuration);
			//var app = builder.Build();
			//Configure(app);
			//app.Run();
		}
	}
}