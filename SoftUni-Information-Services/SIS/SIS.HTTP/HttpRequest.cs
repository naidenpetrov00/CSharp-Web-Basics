namespace SIS.HTTP
{
	using System.Web;
	using System.Text;
	using System.Collections.Generic;

	public class HttpRequest
	{
		public HttpRequest(string httpRequestAsString)
		{
			this.Headers = new List<Header>();
			this.Cookies = new List<Cookie>();
			//remove
			//this.SessionData = new Dictionary<string, string>();

			var lines = httpRequestAsString.Split(
				new string[] { HttpConstants.NewLine },
				StringSplitOptions.None);

			var httpInfoHeader = lines[0];
			var infoHeaderParts = httpInfoHeader.Split(' ');
			if (infoHeaderParts.Length != 3)
			{
				throw new HttpServerException("Invalid HTTP header line.");
			}

			//Method
			var httpMethod = infoHeaderParts[0];
			this.Method = httpMethod switch
			{
				"POST" => HttpMethodType.POST,
				"GET" => HttpMethodType.GET,
				"DELETE" => HttpMethodType.DELETE,
				"PUT" => HttpMethodType.PUT,
				_ => HttpMethodType.Unknown
			};

			//Path
			this.Path = infoHeaderParts[1];

			//Version
			var httpVersion = infoHeaderParts[2];
			this.Version = httpVersion switch
			{
				"HTTP/1.0" => HttpVersionType.Http10,
				"HTTP/1.1" => HttpVersionType.Http11,
				"HTTP/2.0" => HttpVersionType.Http20,
				_ => HttpVersionType.Http10,
			};

			var isInHeader = true;
			var bodyBuilder = new StringBuilder();
			for (int i = 1; i < lines.Length; i++)
			{
				var line = lines[i];
				if (String.IsNullOrWhiteSpace(line))
				{
					isInHeader = false;
					continue;
				}
				if (isInHeader)
				{
					var headerParts = line.Split(new string[] { ": " },
						2,
						StringSplitOptions.None);
					if (headerParts.Length != 2)
					{
						throw new HttpServerException($"Invalid header: {line}");
					}

					var header = new Header(headerParts[0], headerParts[1]);
					this.Headers.Add(header);

					if (headerParts[0] == "Cookie")
					{
						var cookiesAsString = headerParts[1];
						var cookies = cookiesAsString
							.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
						foreach (var cookie in cookies)
						{
							var cookieParts = cookie.Split(new char[] { '=' }, 2);
							if (cookieParts.Length == 2)
							{
								this.Cookies.Add(new Cookie(cookieParts[0], cookieParts[1]));
							}
						}
					}
				}
				else
				{
					bodyBuilder.AppendLine(line);
				}
			}

			this.Body = bodyBuilder.ToString().TrimEnd('\r', '\n');
			this.FormData = new Dictionary<string, string>();
			ParseData(this.FormData, this.Body);

			this.Query = string.Empty;
			if (this.Path.Contains("?"))
			{
				var parts = this.Path.Split(new char[] { '?' }, 2);
				this.Path = parts[0];
				this.Query = parts[1];
			}

			this.QueryData = new Dictionary<string, string>();
			ParseData(this.QueryData, this.Query);
		}

		private void ParseData(IDictionary<string, string> output, string input)
		{
			var dataParts = input.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var dataPart in dataParts)
			{
				var parameterParts = dataPart.Split(new char[] { '=' }, 2);
				output.Add(
					HttpUtility.UrlDecode(parameterParts[0]),
					HttpUtility.UrlDecode(parameterParts[1]));
			}
		}

		public HttpMethodType Method { get; set; }

		public string Path { get; set; }

		public HttpVersionType Version { get; set; }

		public IList<Header> Headers { get; set; }

		public IList<Cookie> Cookies { get; set; }

		public string Query { get; set; }

		public IDictionary<string, string> QueryData { get; set; }

		public string Body { get; set; }

		public IDictionary<string, string> FormData { get; set; }

		public IDictionary<string, string> SessionData { get; set; }
	}
}
