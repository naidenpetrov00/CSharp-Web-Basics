namespace SulsApp
{
	using SIS.MvcFramework;
	using SulsApp.Models;

	public class Program
	{
		public static async Task Main()
		{
			await WebHost.Start(new ServiceConfig());
		}
	}
} 