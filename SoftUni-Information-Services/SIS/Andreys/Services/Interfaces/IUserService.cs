namespace Andreys.Services.Interfaces
{
	public interface IUserService
	{
		string CreateUser(string username, string email, string password);
		string GetUserId(string username, string password);
		void SignIn(string username, string password);

		void SignOut();
	}
}
