namespace SIS.MvcFramework
{
	using DemoApp;
	using SIS.HTTP;
	using System.Runtime.CompilerServices;

	public abstract class Controller
	{
		protected HttpResponse View([CallerMemberName] string viewName = null)
		{
			var layout = File.ReadAllText("Views/Shared/_Layout.html");
			var controllerName = this.GetType().Name.Replace("Controller", string.Empty);
			var html = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");
			var bodyWithLayout = layout.Replace("@RenderBody()", html);

			return new HtmlResponse(bodyWithLayout);
		}
	}
}
