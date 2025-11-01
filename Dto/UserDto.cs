using LanguageCards.Entities;

namespace LanguageCards.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public ICollection<Theme>? AddedThemes { get; set; } = new List<Theme>();
    }
}
