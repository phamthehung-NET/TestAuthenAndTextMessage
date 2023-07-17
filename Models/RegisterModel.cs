using System.ComponentModel.DataAnnotations;

namespace TestAuthenAndTextMessage.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

    }
}
