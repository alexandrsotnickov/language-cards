namespace LanguageCards.Entities
{
    public class Theme
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }

        public string OwnerName { get; set; }
        public User Owner { get; set; }



        public ICollection<User> ThemeSubscribers { get; set; } = new List<User>();
        public int LastCardId { get; set; }



    }
}
