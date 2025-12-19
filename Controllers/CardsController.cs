using LanguageCards.Api.Dto;
using LanguageCards.Dto;
using LanguageCards.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRestApi;

namespace LanguageCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public CardsController(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CardDto cardDto)
        {
            try
            {
                var theme = _context.Themes.FirstOrDefault(t => t.Id == cardDto.ThemeId);
                var card = new Card
                {
                    Theme = theme,
                    ThemeId = theme.Id,
                    DescriptivePicture = cardDto.DescriptivePicture,
                    Word = cardDto.Word,
                    Translation = cardDto.Translation,
                    Transcription = cardDto.Transcription,
                };
                _context.Cards.Add(card);
                await _context.SaveChangesAsync();

                return Ok(
                    new ApiResponseDto<object>
                    {
                        Data = new CardDto
                        {
                            Id = card.Id,
                            Word = card.Word,
                            ThemeId = card.Id,
                            Transcription = card.Transcription,
                            Translation = card.Translation,
                        },

                    }
                );
            }
            catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx)
            {
                if (pgEx.Message.Contains("ограничение уникальности"))
                {
                    return BadRequest(
                        $"Ошибка создания карточки: карточка с таким словом уже существует"
                    );
                }
                else
                {
                    return BadRequest($"Ошибка создания карточки: {pgEx.MessageText}");
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCard(int id)
        {
            var card = _context.Cards.FirstOrDefault(c => c.Id == id);
            if (card == null)
                return NotFound();

            _context.Cards.Remove(card);
            _context.SaveChanges();

            return Ok("Карта была успешно удалена");
        }
    }
}
