using System.ComponentModel.DataAnnotations;

namespace Wajeeh.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}