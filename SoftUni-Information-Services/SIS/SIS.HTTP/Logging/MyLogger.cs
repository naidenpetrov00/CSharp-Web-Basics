namespace SIS.HTTP.Logging
{
	public class MyLogger : ILogger
	{
		public void Log(string message)
		{
			Console.WriteLine("******" + message + "******");
		}
	}
}
