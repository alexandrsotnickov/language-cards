using Humanizer;
using LanguageCards.Dto;
using LanguageCards.Entities;
using LanguageCards.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public IActionResult Create([FromBody] CardDto cardDto)
        {
            try
            {
                
                var theme = _context.Themes.FirstOrDefault(
                    t => t.Name == cardDto.ThemeName
                    && t.Owner.UserName == User.Identity.Name);
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
                _context.SaveChanges();
            }
            catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx)
            {
                if (pgEx.Message.Contains("ограничение уникальности"))
                {
                    return BadRequest($"Ошибка создания карточки: карточка с таким словом уже существует");
                }
                else
                {
                    return BadRequest($"Ошибка создания карточки: {pgEx.MessageText}");
                }
            }

            return Ok("Создание языковой карточки прошло успешно.");
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
