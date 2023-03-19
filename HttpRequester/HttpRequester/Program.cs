namespace HttpRequester
{
	using System.Net;
	using System.Net.Sockets;

	public class Program
	{
		public static async Task Main(string[] args)
		{
			var tcpListener = new TcpListener(IPAddress.Loopback, 1234);

			while (true)
			{
				var tcpCLient = tcpListener.AcceptTcpClient();
				var networkStream = tcpCLient.GetStream();

				// TODO: Use buffer
				var requestBytes = new byte[100000];
				var bytesRead = networkStream.Read(requestBytes, 0, requestBytes.Length);
			}
		}

		public static async Task HttpRequester()
		{

			var client = new HttpClient();
			var responce = await client.GetAsync("https://softuni.bg/");
			var result = await responce.Content.ReadAsStringAsync();

			File.WriteAllText("index.html", result);
		}
	}
}