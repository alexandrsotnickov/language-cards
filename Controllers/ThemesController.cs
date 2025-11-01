using LanguageCards.Dto;
using LanguageCards.Entities;
using LanguageCards.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestApi;

namespace LanguageCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ThemesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        
        public ThemesController(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] ThemeDto themeDto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                var theme = new Theme { Name = themeDto.Name, Owner = user, OwnerId = user.Id, OwnerName = user.UserName, ThemeSubscribers = { user } };
                _context.Themes.Add(theme);
            
                _context.SaveChanges();
            }
            catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx)
            {
                if(pgEx.Message.Contains("ограничение уникальности"))
                {
                    return BadRequest($"Ошибка создания темы: коллекция с таким именем уже существует");
                }
                else
                {
                    return BadRequest($"Ошибка создания темы: {pgEx.MessageText}");
                }
            }

            return Ok("Создание темы прошло успешно.");
        }


        [HttpGet("{themeId}/random-card")]
        public IActionResult GetRandomCard(int themeId)
        {

            var card = _context.Cards
                .Include(c => c.Theme)
                .Where(card => card.Theme.LastCardId != card.Id &&
                        (!_context.UserCardsStatuses.Any(u => u.CardId == card.Id)
                        || _context.UserCardsStatuses.Any(u => u.CardId == card.Id
                        && u.Status == CardStatus.NotStudied))
                       && card.ThemeId == themeId)
                .OrderBy(card => EF.Functions.Random()).FirstOrDefault();

            card.Theme.LastCardId = card.Id;
            _context.SaveChanges();

            return Ok(card);
        }


        [HttpGet("{themeId}/cards")]
        public IActionResult GetCardsByTheme(int themeId)
        {
            var cards = _context.Cards
                .Where(c => c.ThemeId == themeId)
                .ToList();

            return Ok(cards);
        }


        [HttpDelete("{themeId}")]
        public IActionResult DeleteTheme(int themeId)
        {
            var theme = _context.Themes.Find(themeId);
            if (theme != null
                && theme.OwnerId == _context.Users.FirstOrDefault
                    (u => u.UserName == User.Identity.Name).Id)
            {
                _context.Themes.Remove(theme);
                _context.SaveChanges();
                return Ok("Тема и её содержимое были успешно удалены");
            }
            else if(theme == null)
            {
                return BadRequest("Данной темы не существует");
            }
                return BadRequest("Текущий пользователь не является владельцем темы");
        }
    }
}
