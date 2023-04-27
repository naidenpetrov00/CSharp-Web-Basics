namespace SulsApp.Controllers
{
	using SIS.HTTP;
	using SIS.MvcFramework;
	using SulsApp.Services;

	public class ProblemsController : Controller
	{
		private readonly IProblemsService problemsService;

		public ProblemsController(IProblemsService problemsService)
		{
			this.problemsService = problemsService;
		}

		public HttpResponse Create()
		{
			return this.View();
		}

		[HttpPost]
		public HttpResponse Create(string name, int points)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return this.Error("Invalid name");
			}
			if (points <= 0 || points > 100)
			{
				return this.Error("Points range : 0 to 100");
			}


			this.problemsService.CreateProblem(name, points);

			return this.Redirect("/");
		}
	}
}
