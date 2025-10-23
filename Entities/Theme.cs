
namespace LanguageCards.Entities
{
    public class Theme
    {
        public string Id { get; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public User Owner { get; set; }

       
    }
}
