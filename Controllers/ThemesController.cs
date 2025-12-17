using LanguageCards.Api.Dto;
using LanguageCards.Dto;
using LanguageCards.Entities;
using LanguageCards.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
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
        public async Task<IActionResult> Create([FromBody] ThemeDto themeDto)
        {
            try
            {
                if (themeDto.Name == string.Empty || themeDto.Name == null)
                {
                    return BadRequest(
                        new ApiResponseDto<object>
                        {
                            Success = false,
                            ValidationError = $"Ошибка: введите название создаваемой темы"
                        });
                }
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                var theme = new Theme
                {
                    Name = themeDto.Name,
                    Owner = user,
                    OwnerId = user.Id,
                    OwnerName = user.UserName,
                    ThemeSubscribers = { user }
                };

                _context.Themes.Add(theme);

                await _context.SaveChangesAsync();
                CreatedAtAction(nameof(GetThemeById), new { id = theme.Id }, theme);

                return Ok(
               new ApiResponseDto<object>
               {
                   Success = true,
                   Message = "Создание темы прошло успешно.",
                   Data = new ThemeDto { Id = theme.Id, Name = theme.Name, OwnerName = theme.OwnerName },
               });


            }
            catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx)
            {
                if (pgEx.Message.Contains("ограничение уникальности"))
                {
                    return BadRequest(
                        new ApiResponseDto<object>
                        {
                            Success = false,
                            ValidationError = $"Ошибка создания темы: коллекция с таким именем уже существует"
                        });
                }
                else
                {
                    return BadRequest(
                        new ApiResponseDto<object>
                        {
                            Success = false,
                            ValidationError = $"Ошибка создания темы: {pgEx.MessageText}"
                        });
                }
            }


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Theme>> GetThemeById(int id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
                return NotFound(new ApiResponseDto<object>
                {
                    Success = false,
                    ValidationError = $"Ошибка: такой темы не существует"
                });

            return theme;
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


        [HttpGet]
        public IActionResult GeAllThemes()
        {
            var themes = _context.Themes
                .ToList();

            return Ok(themes);
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
            else if (theme == null)
            {
                return BadRequest("Данной темы не существует");
            }
            return BadRequest("Текущий пользователь не является владельцем темы");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheme(int id, [FromBody] ThemeDto dto)
        {
            var theme = await _context.Themes.FirstOrDefaultAsync(t => t.Id == id);

            theme.Name = dto.Name;

            await _context.SaveChangesAsync();

            return Ok(new ApiResponseDto<object>
            {
                Success = true,
                Message = "Обновление названия темы прошло успешно.",
                Data = new ThemeDto { Id = theme.Id, Name = theme.Name, OwnerName = theme.OwnerName },
            });
        }
    }
}
