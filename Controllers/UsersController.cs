using LanguageCards.Api.Dto;
using LanguageCards.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestApi;

namespace LanguageCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public UsersController(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        [HttpPut("subscribe")]
        public IActionResult SubscribeToTheme([FromBody] ThemeDto themeDto)
        {
            var theme = _context.Themes
                .Include(t => t.Owner)
                .Include(t => t.ThemeSubscribers)
                .FirstOrDefault(t => t.Id == themeDto.Id);
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (theme.Owner.UserName == User.Identity.Name)
            {
                return BadRequest("Вы являетесь создателем темы");
            }

            if (theme.ThemeSubscribers
                    .Any(ts => ts.UserName == currentUser.UserName))
            {
                return BadRequest("Вы уже подписались на эту тему");
            }

            theme.ThemeSubscribers.Add(currentUser);
            _context.SaveChanges();
            return Ok();

        }


        [HttpPut("unsubscribe")]
        public IActionResult UnsubscribeTheme([FromBody] ThemeDto themeDto)
        {
            var theme = _context.Themes
                 .Include(t => t.Owner)
                 .Include(t => t.ThemeSubscribers)
                 .FirstOrDefault(t => t.Id == themeDto.Id);
            var currentUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (theme.ThemeSubscribers.Any(ts => ts.UserName == currentUser.UserName))
            {
                theme.ThemeSubscribers.Remove(currentUser);
                _context.SaveChanges();
                return Ok(new ApiResponseDto<object> { Status = 200 });
            }
            else
            {
                return NotFound();
            }


        }


        [HttpGet("themes")]
        public async Task<IActionResult> GetSubscribedThemes()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var currentUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (currentUser == null)
                return NotFound();

            var themes = await _context.Themes
                .AsNoTracking()
                .Where(t => t.OwnerId == currentUser.Id
                            || t.ThemeSubscribers.Any(s => s.Id == currentUser.Id))
                .Select(t => new ThemeDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    OwnerName = t.OwnerName
                })
                .OrderBy(t => t.OwnerName)
                .ThenBy(t => t.Name)
                .ToListAsync();

            return Ok(themes);
        }
    }
}
