using LanguageCards.Api.Entities;
using System.Security.Claims;

namespace LanguageCards.Api.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        RefreshToken CreateRefreshToken(string ip);
    }
}
