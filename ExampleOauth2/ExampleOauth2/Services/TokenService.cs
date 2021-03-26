using ExampleOauth2.Helpers;
using ExampleOauth2.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExampleOauth2.Services
{
    public interface ITokenService
    {
        string GenerateToken(LoginRequestViewModel model);
    }

    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;

        public TokenService(IOptions<AppSettings> appSettings, IUserService userService)
        {
            _appSettings = appSettings.Value;
            _userService = userService;
        }

        public string GenerateToken(LoginRequestViewModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var userValido = _userService.DadosValidos(model.UserName, model.Password);

            if (userValido)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim("UserID", model.Password)
                }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenResult = tokenHandler.WriteToken(token);

                return tokenResult;
            }
            else
            {
                return "";
            }
        }
    }
}
