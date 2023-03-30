namespace DemoApp
{
	using SIS.HTTP;

	public class Program
	{
		static async Task Main(string[] args)
		{
			var httpServer = new HttpServer(80);
			await httpServer.StartAsync();
		}
	}
}