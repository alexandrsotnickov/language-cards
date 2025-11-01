using System.Net.Mime;

namespace LanguageCards.Dto
{
    public class CardDto
    {
        public int Id { get; set; }
        public string Word { get ; set ; }
        public string? Transcription { get ; set; }
        public string Translation { get ; set ; }
        public byte[]? DescriptivePicture { get ; set ; }

        public string ThemeName { get ; set ; }
       
    }
}
