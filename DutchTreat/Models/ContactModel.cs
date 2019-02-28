using System.ComponentModel.DataAnnotations;

namespace DutchTreat.Models
{
    public class ContactModel
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Message is too long")]
        public string Message { get; set; }
    }
}
