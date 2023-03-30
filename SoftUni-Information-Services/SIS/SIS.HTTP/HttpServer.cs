namespace SIS.HTTP
{
	using System.Net.Sockets;
	using System.Net;
	using System.Text;

	public class HttpServer : IHttpServer
	{
		private readonly TcpListener tcpListener;

		//TODO: action
		public HttpServer(int port)
		{
			this.tcpListener = new TcpListener(IPAddress.Loopback, port);
		}

		public async Task ResetAsync()
		{
			this.Stop();
			await this.StartAsync();
		}

		public async Task StartAsync()
		{
			tcpListener.Start();

			while (true)
			{
				var tcpCLient = await tcpListener.AcceptTcpClientAsync();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				Task.Run(() => ProccessClientAsync(tcpCLient));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}

		public void Stop()
		{
			this.tcpListener.Stop();
		}

		private async Task ProccessClientAsync(TcpClient tcpClient)
		{
			using var networkStream = tcpClient.GetStream();

			// TODO: Use buffer 
			var requestBytes = new byte[100000];
			var bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
			var requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
			var request = new HttpRequest(requestAsString);

			var stringContent = Encoding.UTF8.GetBytes("<form method='post'><input name='username' /><input type='submit' /></form>");
			var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
			var headers = "HTTP/1.0 200 OK" + HttpConstants.NewLine +
						"Server: NaidenServer/1.0" + HttpConstants.NewLine +
						"Content-Type: text/html" + HttpConstants.NewLine +
						//"Content-Disposition: attachment; filename=naiden.html" + NewLine +
						"Content-Lenght: " + stringContent.Length + HttpConstants.NewLine +
						"Set-Cookie: user=naiden; Max-Age=3600" + HttpConstants.NewLine +
						HttpConstants.NewLine;
			var headersBytes = Encoding.UTF8.GetBytes(headers);

			await networkStream.WriteAsync(headersBytes);
			await networkStream.WriteAsync(stringContent);
			Console.WriteLine(string.Join("\n", request.Headers));
			Console.WriteLine(string.Join("\n", request.Body));
			Console.WriteLine(new string('=', 60));
		}
	}
}