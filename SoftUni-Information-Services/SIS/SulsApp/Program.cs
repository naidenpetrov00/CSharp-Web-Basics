namespace SulsApp
{
	using SIS.MvcFramework;

	public class Program
	{
		public static async Task Main()
		{
			await WebHost.Start(new ServiceConfig());
		}
	}
}