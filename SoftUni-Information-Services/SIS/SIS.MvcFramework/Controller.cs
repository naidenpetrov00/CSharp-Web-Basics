﻿namespace SIS.MvcFramework
{
	using SIS.HTTP;
	using SIS.HTTP.Response;
	using System.Runtime.CompilerServices;

	public abstract class Controller
	{
		public HttpRequest Request { get; set; }
		public IDictionary<string, string> CurrUser { get; set; }

		public string User =>
		this.Request.SessionData.ContainsKey("UserId") ?
			this.Request.SessionData["UserId"] : null;

		private HttpResponse ViewByName<T>(string viewPath, object viewModel)
		{
			var viewEngine = new ViewEngine();
			var html = File.ReadAllText(viewPath);
			html = viewEngine.GetHtml(html, viewModel, this.User);

			var layout = File.ReadAllText("Views/Shared/_Layout.html");
			var bodyWithLayout = layout.Replace("@RenderBody()", html);
			bodyWithLayout = viewEngine.GetHtml(bodyWithLayout, viewModel, this.User);

			return new HtmlResponse(bodyWithLayout);
		}

		protected HttpResponse View<T>(T viewModel = null, [CallerMemberName] string viewName = null)
			where T : class
		{
			var typeName = this.GetType().Name;
			var controllerName = typeName.Substring(0, typeName.Length - 10);
			var viewPath = "Views/" + controllerName + "/" + viewName + ".html";

			return this.ViewByName<T>(viewPath, viewModel);
		}

		protected HttpResponse View([CallerMemberName] string viewName = null)
		{
			return this.View<object>(null, viewName);
		}

		protected HttpResponse Error(string error)
		{
			return this.ViewByName<ErrorViewModel>("Views/Shared/Error.html", new ErrorViewModel { Error = error, });
		}

		protected HttpResponse Redirect(string url)
		{
			return new RedirectResponse(url);
		}

		protected void SignIn(string userId)
		{
			this.Request.SessionData["UserId"] = userId;
		}

		protected void SignOut()
		{
			this.Request.SessionData["UserId"] = null;
		}
	}
}
