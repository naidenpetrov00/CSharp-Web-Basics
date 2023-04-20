namespace HttpRequester
{
	using System.Net;
	using System.Text;
	using System.Net.Http;
	using System.Net.Sockets;
	using System.Text.RegularExpressions;

	public class Program
	{
		public static async Task Main(string[] args)
		{
			var tcpListener = new TcpListener(IPAddress.Loopback, 80);
			tcpListener.Start();

			while (true)
			{
				var tcpCLient = await tcpListener.AcceptTcpClientAsync();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Task.Run(() => ProccessClientAsync(tcpCLient));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}

		private static async Task ProccessClientAsync(TcpClient tcpClient)
		{
			const string NewLine = "\r\n";

			using var networkStream = tcpClient.GetStream();

			// TODO: Use buffer 
			var requestBytes = new byte[100000];
			var bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
			var request = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
			var fileContent = File.ReadAllBytes("cat.jpg");
			var responseText = "";
			var headers = "HTTP/1.0 200 OK" + NewLine +
						"Server: NaidenServer/1.0" + NewLine +
						"Content-Type: image/jpeg" + NewLine +
						//"Content-Disposition: attachment; filename=naiden.html" + NewLine +
						"Content-Lenght: " + fileContent.Length + NewLine +
						"Set-Cookie: user=naiden; Max-Age=3600" + NewLine +
						NewLine;
			var headersBytes = Encoding.UTF8.GetBytes(headers);

			await networkStream.WriteAsync(headersBytes);
			await networkStream.WriteAsync(fileContent);
			Console.WriteLine(request);
			Console.WriteLine(new string('=', 60));
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