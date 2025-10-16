namespace LanguageCards.Interfaces
{
    public interface IUser
    {
        int Id { get; }
        string Login { get; set; }
        string Password { get; set; }

        
    }
}