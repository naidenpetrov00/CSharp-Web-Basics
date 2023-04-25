namespace SIS.MvcFramework
{
	using System;
	using System.Linq;
	using System.Reflection;
	using SIS.HTTP;
	using SIS.HTTP.Logging;
	using SIS.HTTP.Response;
	using XAct.Messages;

	public class WebHost
	{
		public static async Task StartAsync(IMvcApplication app)
		{

			var routeTable = new List<Route>();
			IServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.Add<ILogger, ConsoleLogger>();
			var logger = serviceCollection.CreateInstance<ILogger>();

			app.ConfigureServices(serviceCollection);
			app.Configure(routeTable);
			AutoRegisterStaticFilesRoutes(routeTable);
			AutoRegisterActionRoutes(routeTable, serviceCollection, app);

			logger.Log("Registered routes:");
			foreach (var route in routeTable)
			{
				logger.Log(route.ToString());
			}

			logger.Log("Requests:");
			var httpServer = new HttpServer(80, routeTable, logger);
			await httpServer.StartAsync();
		}

		private static void AutoRegisterActionRoutes(List<Route> routeTable, IServiceCollection services, IMvcApplication app)
		{
			var types = app
				.GetType()
				.Assembly
				.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(Controller)) && !t.IsAbstract);
			foreach (var type in types)
			{
				var methods = type
					.GetMethods()
					.Where(m => !m.IsConstructor
					&& !m.IsSpecialName
					&& m.IsPublic
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

					routeTable.Add(new Route(httpActionType, url, (request) => InvokeAction(request, services, type, method)));
				}
			}
		}

		private static HttpResponse InvokeAction(HttpRequest request, IServiceCollection services, Type controllerType, MethodInfo actionMethod)
		{
			var controller = services.CreateInstance(controllerType) as Controller;
			controller.Request = request;
			var actionParameterValues = new List<object>();
			var actionParameters = actionMethod.GetParameters();
			foreach (var parameter in actionParameters)
			{
				var parameterName = parameter.Name.ToLower();
				object value = null;
				if (request.QueryData.Any(e => e.Key.ToLower() == parameterName))
				{
					value = request.QueryData.
						FirstOrDefault(e => e.Key.ToLower() == parameterName).Value;
				}
				else if (request.FormData.Any(e => e.Key.ToLower() == parameterName))
				{
					value = request.FormData.
						FirstOrDefault(e => e.Key.ToLower() == parameterName).Value;
				}
				var httpParameter = request.FormData.
					FirstOrDefault(k => string.Compare(k.Key, parameter.Name, true) == 0).Value;
				if (true)
				{

				}
				actionParameterValues.Add(value);
			}
			var response = actionMethod.Invoke(controller, actionParameters.ToArray()) as HttpResponse;

			return response;
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
