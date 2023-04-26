using Microsoft.Extensions.Logging;

namespace SulsApp.ViewModels.Users
{
    public class RegisterInputModel
    {
        public RegisterInputModel(ILogger logger)
        {

        }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}