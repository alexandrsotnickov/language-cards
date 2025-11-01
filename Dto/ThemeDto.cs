using LanguageCards.Entities;
using System.ComponentModel.DataAnnotations;

namespace LanguageCards.Dto
{
    public class ThemeDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? OwnerName  { get; set; }
        
    }
}
