namespace DemoApp
{
	using SIS.HTTP;
	using SIS.HTTP.Response;
	using System;

	public class Program
	{
		public static async Task Main(string[] args)
		{
			var db = new ApplicationDbContext();
			db.Database.EnsureCreated();

			var routeTable = new List<Route>();
			routeTable.Add(new Route(HttpMethodType.GET, "/", Index));
			routeTable.Add(new Route(HttpMethodType.POST, "/Tweets/Create", CreateTweet));
			routeTable.Add(new Route(HttpMethodType.GET, "/favicon.ico", FavIcon));

			var httpServer = new HttpServer(80, routeTable);
			await httpServer.StartAsync();
		}

		private static HttpResponse CreateTweet(HttpRequest request)
		{
			var db = new ApplicationDbContext();
			db.Tweets.Add(new Tweet
			{
				CreatedOn = DateTime.UtcNow,
				Creator = request.FormData["creator"],
				Content = request.FormData["tweetName"],
			});
			db.SaveChanges();

			return new HtmlResponse("Thank you for your tweet <3");
		}

		private static HttpResponse FavIcon(HttpRequest request)
		{
			var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
			return new FileResponse(byteContent, "image/x-icon");
		}

		public static HttpResponse Index(HttpRequest request)
		{
			return new HtmlResponse($"<form action='/Tweets/Create' method='Post'><input name='creator' /><br /><textarea name='tweetName'></textarea><br /><input type='submit' /></form>");
		}
	}
}