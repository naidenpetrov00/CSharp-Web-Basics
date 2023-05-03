namespace Andreys
{
	using SIS.MvcFramework;

	internal class Program
	{
		static async Task Main(string[] args)
		{
			await WebHost.StartAsync(new Startup());
		}
	}
}