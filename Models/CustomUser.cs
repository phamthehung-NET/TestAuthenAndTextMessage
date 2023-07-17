using Microsoft.AspNetCore.Identity;

namespace TestAuthenAndTextMessage.Models
{
    public class CustomUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }
    }
}
