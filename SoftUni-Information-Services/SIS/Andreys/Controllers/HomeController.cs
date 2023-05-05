namespace Andreys.Controllers
{
    using Andreys.Services.Interfaces;
    using SIS.HTTP;
    using SIS.MvcFramework;
    public class HomeController : Controller
	{
		private readonly IHomeService homeService;

		public HomeController(IHomeService homeService)
		{
			this.homeService = homeService;
		}

		[HttpGet("/")]
		public HttpResponse Index()
		{
			return this.View();
		}

		[HttpGet("/Home")]
		public HttpResponse Home()
		{
			var models = this.homeService.GetProducts();

			return this.View(models);
		}
	}
}
