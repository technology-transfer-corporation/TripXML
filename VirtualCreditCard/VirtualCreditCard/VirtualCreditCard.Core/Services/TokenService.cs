using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VirtualCreditCard.Core.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string BuildToken(string username)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration["TokenKey"]));

            var signingCredentials = new SigningCredentials(signingKey,
                SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
            };
            var jwt = new JwtSecurityToken(claims: claims,
                signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }

    public interface ITokenService
    {
        string BuildToken(string username);
    }
}
