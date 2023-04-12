﻿namespace DemoApp
{
	using SIS.HTTP;
	using SIS.HTTP.Response;
	using System;
	using System.Text;
	using System.Collections.Generic;

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

			return new RedirectResponse("/");
		}

		private static HttpResponse FavIcon(HttpRequest request)
		{
			var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
			return new FileResponse(byteContent, "image/x-icon");
		}

		public static HttpResponse Index(HttpRequest request)
		{
			var db = new ApplicationDbContext();
			var tweets = db.Tweets
				.Select(x => new
				{
					x.CreatedOn,
					x.Creator,
					x.Content
				})
				.ToList();

			var html = new StringBuilder();
			html.Append("<table><tr><th>Date</th><th>Creator</th><th>Content</th></tr>");
			foreach (var tweet in tweets)
			{
				html.Append($"<tr><td>{tweet.CreatedOn}</td>" +
					$"<td>{tweet.Creator}</td>" +
					$"<td>{tweet.Content}</td></tr>");
			}
			html.Append("</table>");
			html.Append($"<form action='/Tweets/Create' method='Post'><input name='creator' />" +
				$"<br /><textarea name='tweetName'></textarea><br />" +
				$"<input type='submit' /></form>");

			return new HtmlResponse(html.ToString());
		}
	}
}