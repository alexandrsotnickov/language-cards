using LanguageCards.Dto;
using LanguageCards.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRestApi;

namespace LanguageCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserCardStatusController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public UserCardStatusController(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }


        [HttpPost]
        public IActionResult UpdateUserCardStatus([FromBody] CardStatusDto cardstatusDto)
        {
            if (_context.UserCardsStatuses.Any(uc => uc.CardId == cardstatusDto.CardId))
            {
                var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                var userCard = _context.UserCardsStatuses.Find(userId, cardstatusDto.CardId);
                userCard.Status = cardstatusDto.Status;
                _context.SaveChanges();
            }
            else
            {
                var userCard = new UserCardStatus
                {
                    CardId = cardstatusDto.CardId,
                    Status = cardstatusDto.Status,
                    UserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id
                };
                _context.UserCardsStatuses.Add(userCard);
                _context.SaveChanges();
            }

            return Ok();
        }






    }
}
