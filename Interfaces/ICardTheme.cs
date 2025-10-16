namespace LanguageCards.Interfaces
{
    public interface ICardTheme
    {
        string Id { get; }
        string Name { get; set; }
        IUser Owner { get; set; }

       


    }
}
