namespace SIS.MvcFramework
{
	using System;
	using DemoApp;
	using SIS.HTTP;
	using SIS.HTTP.Response;

	public class WebHost
	{
		public static async Task Start(IMvcApplication app)
		{

			var routeTable = new List<Route>();
			app.ConfigureServices();
			app.Configure(routeTable);
			AutoRegisterStaticFilesRoutes(routeTable);
			AutoRegisterActionRoutes(routeTable, app);

			Console.WriteLine("Registered routes:");
			foreach (var route in routeTable)
			{
				Console.WriteLine(route);
			}

			Console.WriteLine("Requests:");
			var httpServer = new HttpServer(80, routeTable);
			await httpServer.StartAsync();
		}

		private static void AutoRegisterActionRoutes(List<Route> routeTable, IMvcApplication app)
		{
			var types = app
				.GetType()
				.Assembly
				.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(Controller)) && !t.IsAbstract);
			foreach (var type in types)
			{
				Console.WriteLine(type.FullName);
				var methods = type
					.GetMethods()
					.Where(m => !m.IsConstructor
					&& !m.IsSpecialName
					&& m.DeclaringType == type);
				foreach (var method in methods)
				{
					var url = "/" + type.Name.Replace("Controller", string.Empty) + "/" + method.Name;
					var attribute = method
						.GetCustomAttributes(true)
						.FirstOrDefault(a => a.GetType().IsSubclassOf(typeof(HttpMethodAttribute)))
						as HttpMethodAttribute;
					var httpActionType = HttpMethodType.GET;
					if (attribute != null)
					{
						httpActionType = attribute.Type;
						if (attribute.Url != null)
						{
							url = attribute.Url;
						}
					}

					routeTable.Add(new Route(httpActionType, url, (request) =>
					{
						var controller = Activator.CreateInstance(type) as Controller;
						var response = method.Invoke(controller, new[] { request }) as HttpResponse;
						return response;
					}));
					Console.WriteLine("		" + url);
				}
			}
		}

		private static void AutoRegisterStaticFilesRoutes(List<Route> routeTable)
		{
			var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);

			foreach (var staticFile in staticFiles)
			{
				var path = $"/{staticFile.Replace("\\", "/")}";

				routeTable.Add(new Route(HttpMethodType.GET, path, (request) =>
				{
					var fileInfo = new FileInfo(staticFile);
					var contentType = fileInfo.Extension switch
					{
						".css" => "text/css",
						".html" => "text/html",
						".js" => "text/javascript",
						".ico" => "image/x-icon",
						".jpg" => "image/jpg",
						".jpeg" => "image/jpeg",
						".gif" => "image/gif",
						_ => ".text/plain",
					};

					return new FileResponse(File.ReadAllBytes(staticFile), contentType);
				}));
			}
		}
	}
}
