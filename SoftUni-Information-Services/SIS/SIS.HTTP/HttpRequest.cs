namespace SIS.HTTP
{
	using System.Collections.Generic;
	using System.Text;

	public class HttpRequest
	{
		public HttpRequest(string httpRequestAsString)
		{
			this.Headers = new List<Header>();
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
			this.Methood = httpMethod switch
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
				}
				else
				{
					bodyBuilder.AppendLine(line);
				}
			}
		}

		public HttpMethodType Methood { get; set; }

		public string Path { get; set; }

		public HttpVersionType Version { get; set; }

		public IList<Header> Headers { get; set; }

		public string Body { get; set; }
	}
}
