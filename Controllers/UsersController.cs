using LanguageCards.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            
            if(theme.ThemeSubscribers
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

            if (theme.ThemeSubscribers.Any(ts => ts.UserName == currentUser.UserName)){
                theme.ThemeSubscribers.Remove(currentUser);
                _context.SaveChanges();
                return Ok();
            }
            else 
            {
                return NotFound();
            }
            
           
        }

        [HttpGet("themes")]
        
        public IActionResult GetSubscribedThemes()
        {
            List<ThemeDto> themesIdList = new List<ThemeDto>();
            foreach (var item in _context.Users
                .Include(u => u.AddedThemes)
                .ThenInclude(t => t.Owner)
                .FirstOrDefault(u => u.UserName == User.Identity.Name)
                .AddedThemes.OrderBy(x => x.OwnerName))
            {
                themesIdList.Add(new ThemeDto { Id = item.Id, Name = item.Name, OwnerName = item.Owner.UserName});
            }
            
            return Ok(themesIdList);
        }
    }
}
