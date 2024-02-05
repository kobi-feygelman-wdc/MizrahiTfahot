using IO.Swagger.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace IO.Swagger.Services
{
    public class AuthService : IAuthService
    {
        public static User user = new User();
        ILogger _logger;
        IConfiguration _configuration;

        protected string errMsg = "";
        public string ErrorMessage => errMsg;

        public AuthService(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        ///  Registers a new user in the system
        /// </summary>
        /// <param name="request"></param>
        /// <returns>user object</returns>
        public async Task<User> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.UserName;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return user;
        }

        /// <summary>
        /// Generates password hash and password salt from a given password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        protected void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies the password 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns>True if succeeds or false otherwise</returns>
        protected bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        /// <summary>
        /// Generates the token from secret key, issuer, audience and expiration date using HmacSha512 algorithm
        /// </summary>
        /// <param name="user"></param>
        /// <returns>token</returns>
        protected string CreateToken(User user)
        {
            try
            {
                var jwtSection = _configuration.GetSection("JwtSettings");
                var secretKey = Encoding.UTF8.GetBytes(jwtSection["SecretKey"].PadRight((512 / 8), '\0'));

                var securityKey = new SymmetricSecurityKey(secretKey);

                string issuer = jwtSection["Issuer"];
                string audience = jwtSection["Audience"];
                int exiprationMins = Int32.Parse(jwtSection["ExpirationInMinutes"]);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Aud, issuer),
                    new Claim(JwtRegisteredClaimNames.Iss, audience),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: new DateTimeOffset(DateTime.Now.AddMinutes(exiprationMins)).DateTime,
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512)
                    );


                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                ValidateToken(secretKey, jwt, issuer, audience);

                return jwt;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateToken error: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Validates the token after creation
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="jwt"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <returns>True if succeeds or false otherwise</returns>
        protected bool ValidateToken(byte[] secretKey, string jwt, string issuer, string audience)
        {
            try
            {
                var hmac = new HMACSHA512(secretKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(hmac.Key)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(jwt, validationParameters, out var validToken);
                _logger.LogInformation($"Token validation successful");

                return principal.Identity.IsAuthenticated;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Token validation failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Logins the user and returns the token upon completion
        /// </summary>
        /// <param name="request"></param>
        /// <returns>token</returns>
        public async Task<string> Login(UserDto request)
        {
            if (user.Username != request.UserName)
            {
                _logger.LogError("Login failed, user not found");
                 errMsg = "User not found";
                return null;
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogError("Login failed, wrong password");
                errMsg = "Wrong password";
                return null;
            }

            string token = CreateToken(user);
            _logger.LogInformation($"token generated: {token}");

            return token;
        }
    }
}
