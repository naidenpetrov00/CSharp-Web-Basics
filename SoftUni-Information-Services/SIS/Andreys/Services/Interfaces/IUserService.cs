namespace Andreys.Services.Interfaces
{
	public interface IUserService
	{
		void CreateUser(string username, string email, string password);

		void SignIn(string username, string password);

		void SignOut();
	}
}
