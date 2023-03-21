namespace HttpRequester
{
	using System.Net;
	using System.Text;
	using System.Net.Sockets;

	public class Program
	{
		public static async Task Main(string[] args)
		{
			const string NewLine = "\r\n";
			var tcpListener = new TcpListener(IPAddress.Loopback, 80);
			tcpListener.Start();

			while (true)
			{
				var tcpCLient = tcpListener.AcceptTcpClient();
				using var networkStream = tcpCLient.GetStream();

				// TODO: Use buffer 
				var requestBytes = new byte[100000];
				var bytesRead = networkStream.Read(requestBytes, 0, requestBytes.Length);
				var request = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);

				var responseText = @"<form action='/Account/Login' method='post'>
<input type=text name='username'/>
<input type=password name='password'/>
<input type=submit value='Login'/>
</form>";

				var response = "HTTP/1.0 200 OK" + NewLine +
							"Server: NaidenServer/1.0" + NewLine +
							"Content-Type: text/html" + NewLine +
							//"Content-Disposition: attachment; filename=naiden.html" + NewLine +
							"Content-Lenght: " + responseText.Length + NewLine +
							NewLine +
							responseText;
				var responseBytes = Encoding.UTF8.GetBytes(response);

				networkStream.Write(responseBytes, 0, responseBytes.Length);

				Console.WriteLine(request);
				Console.WriteLine(new string('=', 60));
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