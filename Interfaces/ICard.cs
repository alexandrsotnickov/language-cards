using static System.Net.Mime.MediaTypeNames;

namespace LanguageCards.Interfaces
{
    public interface ICard
    {
        string Word { get; set; }
        string Transcription { get; set; }
        string Translation { get; set; }
        byte[] DescriptivePicture { get; set; }

        ICardTheme Theme { get; set; }

        
    }
}
