using LanguageCards.Interfaces;

namespace LanguageCards.Entities
{
    public class CardTheme : ICardTheme
    {
        public string Id { get; }
        public string Name { get; set; }
        public IUser Owner { get; set; }

       
    }
}
