using LanguageCards.Api.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LanguageCards.Api.Services
{
    public class TokenService : ITokenService
    {
        
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                 issuer: jwtSettings["Issuer"],
                 audience: jwtSettings["Audience"],
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(15),
                 signingCredentials: creds
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken CreateRefreshToken(string ip)
        {
            var rt = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Created = DateTime.UtcNow,
                CreatedByIp = ip
            };
            return rt;
        }
    }
}
