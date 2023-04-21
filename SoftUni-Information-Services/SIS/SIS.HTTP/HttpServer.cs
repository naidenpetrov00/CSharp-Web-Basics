namespace SIS.HTTP
{
	using System.Net.Sockets;
	using System.Net;
	using System.Text;
	using System;

	public class HttpServer : IHttpServer
	{
		private readonly TcpListener tcpListener;
		private readonly IList<Route> routeTable;
		private readonly IDictionary<string, IDictionary<string, string>> sessions;

		//TODO: action
		public HttpServer(int port, IList<Route> routeTable)
		{
			this.tcpListener = new TcpListener(IPAddress.Loopback, port);
			this.routeTable = routeTable;
			this.sessions = new Dictionary<string, IDictionary<string, string>>();
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

			try
			{
				// TODO: Use buffer 
				var requestBytes = new byte[100000];
				var bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
				var requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
				var request = new HttpRequest(requestAsString);
				var sessionCookie = request.Cookies.FirstOrDefault(c => c.Name == HttpConstants.SessionIdCookieName);
				string? newSessionId = null;

				if (sessionCookie != null && this.sessions.ContainsKey(sessionCookie.Value))
				{
					request.SessionData = this.sessions[sessionCookie.Value];
				}
				else
				{
					newSessionId = Guid.NewGuid().ToString();
					var dictionary = new Dictionary<string, string>();
					this.sessions.Add(newSessionId, dictionary);
					request.SessionData = dictionary;
				}
				Console.WriteLine($"{request.Methood} {request.Path}");

				HttpResponse response;
				var route = this.routeTable
					.FirstOrDefault(r => r.HttpMethod == request.Methood &&
					string.Compare(r.Path, request.Path, true) == 0);
				if (route == null)
				{
					response = new HttpResponse(HttpResponseCode.NotFound, new byte[0]);
				}
				else
				{
					response = route.Action(request);
				}

				response.Headers.Add(new Header("Server", "NaidenServer/1.0"));


				if (newSessionId != null)
				{
					response.Cookies.Add(new ResponseCookie(HttpConstants.SessionIdCookieName, newSessionId)
					{
						Secure = true,
						HttpOnly = true,
						MaxAge = 30 * 3600,
					});
				}

				var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

				await networkStream.WriteAsync(responseBytes);
				await networkStream.WriteAsync(response.Body);
			}
			catch (Exception ex)
			{
				var errorResponse = new HttpResponse(
					HttpResponseCode.InternalServerError,
					Encoding.UTF8.GetBytes(ex.ToString()));
				errorResponse.Headers.Add(new Header("Content-Type", "text/plain"));
				var responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());

				await networkStream.WriteAsync(responseBytes);
				await networkStream.WriteAsync(errorResponse.Body);
			}
		}
	}
}