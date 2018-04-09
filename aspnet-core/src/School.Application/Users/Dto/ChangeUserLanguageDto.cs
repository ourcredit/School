using System.ComponentModel.DataAnnotations;

namespace School.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}