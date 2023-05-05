namespace Andreys.Controllers
{
	using Andreys.Data.Enums;
	using Andreys.Models;
	using Andreys.Services.Interfaces;
	using Andreys.ViewModels;
	using SIS.HTTP;
	using SIS.MvcFramework;
	using System.Diagnostics;

	public class ProductsController : Controller
	{
		private readonly IProductService productService;

		public ProductsController(IProductService productService)
		{
			this.productService = productService;
		}

		public HttpResponse Add()
		{
			return this.View();
		}

		[HttpPost]
		public HttpResponse Add(ProductInputModel input)
		{
			this.productService.AddProduct(input);

			return this.Redirect("/");
		}
	}
}
