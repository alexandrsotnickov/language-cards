using LanguageCards.Interfaces;
using System.Net.Mime;

namespace LanguageCards.Entities
{
    public class Card : ICard
    {
        public string Word { get ; set ; }
        public string Transcription { get ; set; }
        public string Translation { get ; set ; }
        public byte[] DescriptivePicture { get ; set ; }
        public ICardTheme Theme { get; set; }

       
    }
}
