namespace HttpRequester
{
	using System.Net;
	using System.Text;
	using System.Net.Sockets;
	using System.Net.Http;
	using System.Text.RegularExpressions;

	public class Program
	{
		public static async Task Main(string[] args)
		{
			const string NewLine = "\r\n";
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
			var username = Regex.Match(request, @"Cookie: user=[^\n]*\n").Value;
			var responseText = "<h1>" + username + "</h1>";
			var response = "HTTP/1.0 200 OK" + NewLine +
						"Server: NaidenServer/1.0" + NewLine +
						"Content-Type: text/html" + NewLine +
						//"Content-Disposition: attachment; filename=naiden.html" + NewLine +
						"Content-Lenght: " + responseText.Length + NewLine +
						"Set-Cookie: user=naiden; Max-Age=3600" + NewLine +
						NewLine +
						responseText;
			var responseBytes = Encoding.UTF8.GetBytes(response);

			await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);

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