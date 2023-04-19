namespace SIS.MvcFramework
{
	using DemoApp;
	using SIS.HTTP;
	using System.Runtime.CompilerServices;

	public abstract class Controller
	{
		protected HttpResponse View<T>(T viewModel = null, [CallerMemberName] string viewName = null)
			where T : class
		{
			var viewEngine = new ViewEngine();

			var controllerName = this.GetType().Name.Replace("Controller", string.Empty);
			var html = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");
			html = viewEngine.GetHtml(html, viewModel);

			var layout = File.ReadAllText("Views/Shared/_Layout.html");
			var bodyWithLayout = layout.Replace("@RenderBody()", html);
			bodyWithLayout = viewEngine.GetHtml(bodyWithLayout, viewModel);

			return new HtmlResponse(bodyWithLayout);
		}

		protected HttpResponse View([CallerMemberName] string viewName = null)
		{
			return this.View<object>(null, viewName);
		}
	}
}
