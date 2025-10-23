
using System.Net.Mime;

namespace LanguageCards.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string Word { get ; set ; }
        public string? Transcription { get ; set; }
        public string Translation { get ; set ; }
        public byte[]? DescriptivePicture { get ; set ; }
        public int ThemeId { get; set; }
        public Theme Theme { get; set; }

       
    }
}
