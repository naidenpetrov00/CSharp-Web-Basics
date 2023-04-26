namespace SIS.MvcFramework
{
	using System;
	using System.Linq;
	using System.Reflection;
	using System.Reflection.Metadata;
	using SIS.HTTP;
	using SIS.HTTP.Logging;
	using SIS.HTTP.Response;
	using XAct;
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

		private static void AutoRegisterActionRoutes(List<Route> routeTable, IServiceCollection serviceCollection, IMvcApplication app)
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

					routeTable.Add(new Route(httpActionType, url, (request) => InvokeAction(request, serviceCollection, type, method)));
				}
			}
		}

		private static HttpResponse InvokeAction(HttpRequest request, IServiceCollection serviceCollection, Type controllerType, MethodInfo actionMethod)
		{
			var controller = serviceCollection.CreateInstance(controllerType) as Controller;
			controller.Request = request;
			var actionParameterValues = new List<object>();
			var actionParameters = actionMethod.GetParameters();
			foreach (var parameter in actionParameters)
			{
				object value = GetValueFromRequest(controller.Request, parameter.Name);

				try
				{
					actionParameterValues.Add(Convert.ChangeType(value, parameter.ParameterType));
				}
				catch
				{
					var parameterValue = serviceCollection.CreateInstance(parameter.ParameterType);
					foreach (var property in parameter.ParameterType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
					{
						var propertyValue = GetValueFromRequest(request, property.Name);
						property.SetValue(parameterValue, Convert.ChangeType(propertyValue, property.PropertyType));
					}

					actionParameterValues.Add(parameterValue);
				}
			}
			var response = actionMethod.Invoke(controller, actionParameterValues.ToArray()) as HttpResponse;

			return response;
		}

		private static object GetValueFromRequest(HttpRequest request, string parameterName)
		{
			parameterName = parameterName.ToLower();
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

			return value;
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
