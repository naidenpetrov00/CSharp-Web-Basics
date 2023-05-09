namespace AppViewNamespace
{
	using System;
	using System.Linq;
	using System.Text;
	using System.Collections.Generic;
	using SIS.MvcFramework;

	public class AppViewCode : IView
	{
		public string GetHtml(object model, string user)
		{
			var Model = model as List<Andreys.ViewModels.ProductViewModel>;
			var User = user;
			var html = new StringBuilder();
			html.AppendLine(@"<h1 class=""text-center""><span class=""badge badge-pill badge-dark"">Products</span></h1>");

			html.AppendLine(@"<div class=""bg-blur container"">");

			html.AppendLine(@"    <div class=""row"">");

			html.AppendLine(@"        <div class=""col-3 text-center"">");

			foreach (var value in Model)
			{
				html.AppendLine(@"            <a href=""/Products/Details?id=" + value.Id + @"""><img src=""" + value.ImageUrl + @""" class=""img-fluid"" width=""200"" height=""200"" /></a>");

				html.AppendLine(@"            <h5 class=""text-center"">" + value.Name + @"</h5>");

				html.AppendLine(@"            <h2 class=""text-center"">" + value.Price + @"</h2>");

			}
			html.AppendLine(@"        </div>");

			html.AppendLine(@"    </div>");

			html.AppendLine(@"</div>");


			return html.ToString();
		}
	}
}