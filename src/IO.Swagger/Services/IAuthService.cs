using IO.Swagger.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IO.Swagger.Services
{
    public interface IAuthService
    {
        string ErrorMessage { get; }
        /// <summary>
        ///  Registers a new user in the system
        /// </summary>
        /// <param name="request"></param>
        /// <returns>user object</returns>
        Task<User> Register(UserDto request);

        /// <summary>
        /// Logins the user and returns the token upon completion
        /// </summary>
        /// <param name="request"></param>
        /// <returns>token</returns>
        Task<string> Login(UserDto request);
    }
}
