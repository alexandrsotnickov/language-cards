using LanguageCards.Enums;

namespace LanguageCards.Entities
{
    public class UserCardStatus
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }

        public CardStatus Status { get; set; }
    }
}
