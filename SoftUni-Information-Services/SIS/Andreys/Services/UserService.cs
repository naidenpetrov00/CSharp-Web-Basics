namespace Andreys.Services
{
	using Andreys.Data;
	using Andreys.Models;
	using Andreys.Services.Interfaces;
	using System.Text;
	using XAct;
	using XSystem.Security.Cryptography;

	public class UserService : IUserService
	{
		public ApplicationDbContext db { get; }

		public UserService(ApplicationDbContext db)
		{
			this.db = db;
		}

		private string Hash(string input)
		{
			var crypt = new SHA256Managed();
			var hash = new StringBuilder();
			byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
			foreach (byte theByte in crypto)
			{
				hash.Append(theByte.ToString("x2"));
			}
			return hash.ToString();
		}

		public void CreateUser(string username, string email, string password)
		{
			var user = new User
			{
				Username = username,
				Email = email,
				Password = this.Hash(password)
			};

			this.db.Users.Add(user);
			this.db.SaveChanges();
		}

		public void SignIn(string username, string password)
		{
			throw new NotImplementedException();
		}

		public void SignOut()
		{
			throw new NotImplementedException();
		}
	}
}
