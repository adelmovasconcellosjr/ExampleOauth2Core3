using Microsoft.Extensions.Configuration;

namespace ExampleOauth2.Services
{
    public interface IUserService
    {
        bool DadosValidos(string login, string password);
    }

    public class UserService : IUserService
    {
        public IConfiguration Configuration { get; }

        public UserService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public bool DadosValidos(string _username, string _password)
        {
            var user = Configuration["AppConfiguration:Username"];
            var pwd = Configuration["AppConfiguration:Password"];


            if (_username.Equals(user) && _password.Equals(pwd))
            {
                return true;
            }

            return false;
        }
    }
}
