namespace Andreys.Controllers
{
	using Andreys.Services.Interfaces;
	using Microsoft.EntityFrameworkCore.Query.Internal;
	using SIS.HTTP;
	using SIS.MvcFramework;

	public class UsersController : Controller
	{
		private readonly IUserService userService;

		public UsersController(IUserService userService)
		{
			this.userService = userService;
		}

		public HttpResponse Login()
		{
			return this.View();
		}

		[HttpPost]
		public HttpResponse Login(string username, string password)
		{
			return this.Redirect("/");
		}

		public HttpResponse Register()
		{
			return this.View();
		}

		[HttpPost]
		public HttpResponse Register(string username, string email, string password, string confirmPassword)
		{
			if (username.Length < 4 || username.Length > 20)
			{
				return this.Redirect("/Users/Register");
			}
			if (password.Length < 6 || password.Length > 20)
			{
				return this.Redirect("/Users/Register");
			}
			if (confirmPassword != password)
			{
				return this.Redirect("/Users/Register");
			}

			this.userService.CreateUser(username, email, password);

			return this.Redirect("/");
		}
	}
}
