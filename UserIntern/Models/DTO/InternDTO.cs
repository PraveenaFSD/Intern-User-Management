using System.ComponentModel.DataAnnotations;

namespace UserIntern.Models.DTO
{
    public class InternDTO:Intern
    {
       [Required(ErrorMessage="Password must be minimum 8 characters long")]
        public string? PasswordClear { get; set; }

    }
}
