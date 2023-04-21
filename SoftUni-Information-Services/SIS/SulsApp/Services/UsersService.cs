namespace SulsApp.Services
{
	using System.Text;
	using SulsApp.Models;
	using XSystem.Security.Cryptography;

	public class UsersService : IUsersService
	{
		private readonly ApplicationDbContext db;

		public UsersService(ApplicationDbContext db)
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

		public void ChangePassword(string username, string password)
		{
			var user = this.db.Users.FirstOrDefault(u => u.Username == username);
			if (user == null)
			{
				return;
			}

			user.Password = this.Hash(password);
			this.db.SaveChanges();
		}

		public int CountUsers()
		{
			return this.db.Users.Count();
		}

		public void CreateUser(string username, string email, string password)
		{
			var user = new User
			{
				Username = username,
				Email = email,
				Password = this.Hash(password),
			};

			db.Users.Add(user);
			db.SaveChanges();
		}

		public bool IsValidUser(string username, string password)
		{
			var passwordHash = this.Hash(password);

			return this.db.Users
				.Any(u => u.Username == username
				&& u.Password == passwordHash);
		}
	}
}
