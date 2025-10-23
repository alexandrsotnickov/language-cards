using LanguageCards.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create([FromBody] Card card)
        {
           _context.Cards.Add(card);

            return Ok("Создание языковой карточки прошло успешно.");
        }
    }
}
