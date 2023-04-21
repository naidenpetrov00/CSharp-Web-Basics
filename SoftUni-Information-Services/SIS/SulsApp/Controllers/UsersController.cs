namespace SulsApp.Controllers
{
	using DemoApp;
	using SIS.HTTP;
	using SIS.MvcFramework;
	using SulsApp.Models;
	using System;
	using System.Net.Mail;
	using System.Text;

	public class UsersController : Controller
	{
		public HttpResponse Login()
		{
			return this.View();
		}

		[HttpPost("/Users/Login")]
		public HttpResponse DoLogin()
		{
			return this.View();
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

			if (username?.Length < 5 || username?.Length > 20)
			{
				return this.Error("Username should be between 5 and 20 characters!");
			}

			if (!IsValidEmailAddress(email))
			{
				return this.Error("Invalid Email");
			}

			if (password != confirmPassword)
			{
				return this.Error("Password should be the same!");
			}
			if (password?.Length < 6 || password?.Length > 20)
			{
				return this.Error("Password should be between 6 and 20 characters!");
			}

			var user = new User
			{
				Username = username,
				Email = email,
				Password = this.Hash(password),
			};

			var db = new ApplicationDbContext();
			db.Users.Add(user);
			db.SaveChanges();

			return this.Redirect("/");
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
	}
}
