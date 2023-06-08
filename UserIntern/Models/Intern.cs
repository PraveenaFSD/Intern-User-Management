using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserIntern.Models
{
    public class Intern
    {
        public Intern()
        {
            Name = string.Empty;
            Gender = "Unknown";
        }

        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public User? User { get; set; }
        [Required(ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        [Required(ErrorMessage = "Gender cannot be empty")]
        public string Gender { get; set; }
        public string? Phone { get; set; }
        [EmailAddress(ErrorMessage = "Please enter the email correctly")]
        public string? Email { get; set; }

    }
}
