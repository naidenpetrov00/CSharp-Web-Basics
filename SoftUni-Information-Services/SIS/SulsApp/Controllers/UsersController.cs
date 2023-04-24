namespace SulsApp.Controllers
{
    using SIS.HTTP;
    using System.Net.Mail;
    using SIS.MvcFramework;
    using SulsApp.Services;
    using SIS.HTTP.Logging;

    public class UsersController : Controller
	{
		private readonly ApplicationDbContext db;
		private IUsersService usersService;
		private ILogger logger;

		public UsersController(IUsersService userService)
		{
			this.db = new ApplicationDbContext();
			this.usersService = userService;
			this.logger = new ConsoleLogger();
		}

		private bool IsValidEmailAddress(string emailAddress)
		{
			try
			{
				var email = new MailAddress(emailAddress);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public HttpResponse Login()
		{
			return this.View();
		}

		[HttpPost("/Users/Login")]
		public HttpResponse DoLogin()
		{
			var username = this.Request.FormData["username"];
			var password = this.Request.FormData["password"];

			var userId = this.usersService.GetUserId(username, password);
			if (userId == null)
			{
				return this.Redirect("/");
			}

			this.SignIn(userId);
			this.logger.Log("User logged in: " + username);
			return this.Redirect("/");
		}

		public HttpResponse Register()
		{
			return this.View();
		}

		[HttpPost("/Users/Register")]
		public HttpResponse DoRegister()
		{
			var username = this.Request.FormData["username"];
			var email = this.Request.FormData["email"];
			var password = this.Request.FormData["password"];
			var confirmPassword = this.Request.FormData["confirm_password"];

			if (password != confirmPassword)
			{
				return this.Error("Password should be the same!");
			}
			if (username?.Length < 5 || username?.Length > 20)
			{
				return this.Error("Username should be between 5 and 20 characters!");
			}
			if (password?.Length < 6 || password?.Length > 20)
			{
				return this.Error("Password should be between 6 and 20 characters!");
			}
			if (!IsValidEmailAddress(email))
			{
				return this.Error("Invalid Email");
			}

			this.usersService.CreateUser(username, email, password);
			this.logger.Log("New user: " + username);

			return this.Redirect("/");
		}

		public HttpResponse Logout()
		{
			this.SignOut();
			return this.Redirect("/");
		}
	}
}
