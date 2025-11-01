
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageCards.Entities
{
    public class User : IdentityUser 
    {
        public ICollection<Theme> AddedThemes { get; set; } = new List<Theme>();
    }

}
