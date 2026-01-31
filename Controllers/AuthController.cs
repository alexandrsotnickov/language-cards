using System.Security.Claims;
using LanguageCards.Api.Dto;
using LanguageCards.Api.Services;
using LanguageCards.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestApi;

namespace LanguageCards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;

        public AuthController(
            UserManager<User> userManager,
            IConfiguration config,
            AppDbContext context,
            SignInManager<User> signInManager,
            ITokenService tokenService
        )
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _context = context;
            _config = config;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
                return BadRequest(
                    new ApiResponseDto<object>
                    {
                        Success = false,
                        ValidationError = "Такой пользователь уже существует",
                    }
                );

            var user = new User { UserName = request.Username };

            var result = await _userManager.CreateAsync(user, request.Password);

            var validationError = result.Errors.FirstOrDefault()?.Description;

            if (!result.Succeeded)
                return BadRequest(
                    new ApiResponseDto<object>
                    {
                        Success = false,
                        ValidationError = validationError,
                    }
                );

            user = await _userManager.FindByNameAsync(user.UserName);

            var theme = new Theme
            {
                Name = "Мои карточки",
                Owner = user,
                OwnerId = user.Id,
                OwnerName = user.UserName,
                ThemeSubscribers = { user },
            };
            _context.Themes.Add(theme);
            _context.SaveChanges();

            return Ok(
                new ApiResponseDto<object>
                {
                    Success = true,
                    Message = "Вы успешно зарегистрировались",
                }
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return Unauthorized(
                    new ApiResponseDto<object>
                    {
                        Success = false,
                        ValidationError = "Пользователь не найден",
                    }
                );

            var check = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                false
            );
            if (!check.Succeeded)
                return Unauthorized(
                    new ApiResponseDto<object>
                    {
                        Success = false,
                        ValidationError = "Неверный пароль",
                    }
                );

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var refreshToken = _tokenService.CreateRefreshToken(ip);
            refreshToken.UserId = user.Id;

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.Expires,
                Path = "/",
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            return Ok(new { AccessToken = accessToken, UserName = user.UserName });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // get refresh token from HttpOnly cookie
            if (!Request.Cookies.TryGetValue("refreshToken", out var token))
                return Unauthorized();

            var rt = await _context
                .RefreshTokens.Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token);
            if (rt == null || rt.IsRevoked || rt.Expires <= DateTime.UtcNow)
                return Unauthorized();

            // optionally rotate token: revoke old, create new
            rt.IsRevoked = true;

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var newRt = _tokenService.CreateRefreshToken(ip);
            newRt.UserId = rt.UserId;
            _context.RefreshTokens.Add(newRt);

            // create new access token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, rt.User.Id),
                new Claim(ClaimTypes.Name, rt.User.UserName),
            };
            var newAccessToken = _tokenService.GenerateAccessToken(claims);
            await _context.SaveChangesAsync();

            // set new cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRt.Expires,
                Path = "/",
            };
            Response.Cookies.Append("refreshToken", newRt.Token, cookieOptions);

            return Ok(new { accessToken = newAccessToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var token))
            {
                var rt = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);
                if (rt != null)
                {
                    rt.IsRevoked = true;
                    await _context.SaveChangesAsync();
                }
            }

            Response.Cookies.Delete(
                "refreshToken",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/",
                }
            );
            return Ok(new { message = "Logged out" });
        }
    }

    public record RegisterRequest(string Username, string Password);

    public record LoginRequest(string Username, string Password);
}
