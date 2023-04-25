namespace SulsApp.Controllers
{
	using SIS.HTTP;
	using SIS.HTTP.Logging;
	using SIS.MvcFramework;
	using SulsApp.ViewModels;

	public class HomeController : Controller
	{
		private ILogger logger;

		public HomeController(ILogger logger)
		{
			this.logger = logger;
		}

		[HttpGet("/")]
		public HttpResponse Index()
		{
			var request = this.Request;
			this.logger.Log("Hello from Index");
			var viewModel = new IndexViewModel
			{
				Message = "Welcome to SULS",
				Year = DateTime.UtcNow.Year
			};
			return this.View(viewModel);
		}
	}
}
