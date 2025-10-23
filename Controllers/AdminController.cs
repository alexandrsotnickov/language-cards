using LanguageCards.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

        private readonly IConfiguration _config;
        public AdminController(IConfiguration config)
        {
            _config = config;
            
        }
  

        //[HttpGet("????")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Get()
        //{
        //    return Ok();
        //}

        //// Доступно всем авторизованным
        //[HttpGet("????")]
        //[Authorize]
        //public IActionResult Get2()
        //{
        //    return Ok();
        //}
    }
}
