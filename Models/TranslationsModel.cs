using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Leet_Translator.Models
{
    public class Translations
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Original Text")]
        public string OrginalText { get; set; }
        [Required]
        [DisplayName("Translated Text")]
        public string TranslatedText { get; set; }
        [Required]
        [DisplayName("Translated To")]
        public string TranslatedTo { get; set; }
        public DateTime createdAt { get; set; } 
    }
}
