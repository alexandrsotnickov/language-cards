using LanguageCards.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRestApi;

namespace LanguageCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult Create([FromBody] Theme theme)
        {
            _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            _context.Themes.Add(theme);


            return Ok("Создание темы прошло успешно.");
        }
    }
}
