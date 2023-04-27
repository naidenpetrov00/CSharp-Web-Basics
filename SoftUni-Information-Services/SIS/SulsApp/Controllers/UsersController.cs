namespace SulsApp.Controllers
{
    using SIS.HTTP;
    using System.Net.Mail;
    using SIS.MvcFramework;
    using SulsApp.Services;
    using SIS.HTTP.Logging;
    using SulsApp.ViewModels.Users;

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

		[HttpPost]
		public HttpResponse Login(string username, string password)
		{
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

		[HttpPost]
		public HttpResponse Register(RegisterInputModel input)
		{
			if (input.Password != input.ConfirmPassword)
			{
				return this.Error("Password should be the same!");
			}
			if (input.Username?.Length < 5 || input.Username?.Length > 20)
			{
				return this.Error("Username should be between 5 and 20 characters!");
			}
			if (input.Password?.Length < 6 || input.Password?.Length > 20)
			{
				return this.Error("Password should be between 6 and 20 characters!");
			}
			if (!IsValidEmailAddress(input.Email))
			{
				return this.Error("Invalid Email");
			}

			this.usersService.CreateUser(input.Username, input.Email, input.Password);
			this.logger.Log("New user: " + input.Username);

			return this.Redirect("/");
		}

		public HttpResponse Logout()
		{
			this.SignOut();
			return this.Redirect("/");
		}
	}
}
