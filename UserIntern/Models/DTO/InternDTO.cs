using System.ComponentModel.DataAnnotations;

namespace UserIntern.Models.DTO
{
    public class InternDTO:Intern
    {
       
        public string? PasswordClear { get; set; }

    }
}
