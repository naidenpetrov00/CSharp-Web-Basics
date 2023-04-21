namespace SulsApp.Controllers
{
	using SIS.HTTP;
	using SIS.MvcFramework;
	using SulsApp.ViewModels;

	public class HomeController : Controller
	{
		[HttpGet("/")]
		public HttpResponse Index()
		{
			var viewModel = new IndexViewModel
			{
				Message = "Welcome to SULS",
				Year = DateTime.UtcNow.Year
			};
			return this.View(viewModel);
		}
	}
}
