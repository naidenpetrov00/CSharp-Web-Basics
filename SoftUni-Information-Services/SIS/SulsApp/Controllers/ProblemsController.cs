namespace SulsApp.Controllers
{
	using SIS.HTTP;
	using SIS.MvcFramework;

	public class ProblemsController : Controller
	{
		public ProblemsController()
		{

		}

		public HttpResponse Create()
		{
			return this.View();
		}
	}
}
