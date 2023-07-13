using Microsoft.AspNetCore.Identity;

namespace TestAuthenAndTextMessage.Models
{
    public class CustomUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
