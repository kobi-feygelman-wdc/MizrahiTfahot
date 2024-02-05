using IO.Swagger.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IO.Swagger.Models;
using Swashbuckle.AspNetCore.Annotations;
using IO.Swagger.Attributes;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging;
using IO.Swagger.Services;

namespace IO.Swagger.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Handles register user request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/ANATAFR/CalcAPI/1.0/Auth/register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            User user = await _authService.Register(request);

            return Ok(user);
        }

        
        /// <summary>
        /// Handles login user request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/ANATAFR/CalcAPI/1.0/Auth/login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            string token = await _authService.Login(request);

            if (token == null)
                return BadRequest(_authService.ErrorMessage);            

            return Ok(new { token });
        }
    }
}
