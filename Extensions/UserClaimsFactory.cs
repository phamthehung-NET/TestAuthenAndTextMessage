using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TestAuthenAndTextMessage.Data;
using TestAuthenAndTextMessage.Models;

namespace TestAuthenAndTextMessage.Extensions
{
    public class UserClaimsFactory : UserClaimsPrincipalFactory<CustomUser, IdentityRole>
    {
        public UserClaimsFactory(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options, ApplicationDbContext _context) : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(CustomUser user)
        {
            var claims = await base.GenerateClaimsAsync(user);

            var userInfo = await UserManager.FindByIdAsync(user.Id);

            claims.AddClaim(new Claim("Email", userInfo.Email));
            claims.AddClaim(new Claim("PhoneNumber", userInfo.PhoneNumber));
            claims.AddClaim(new Claim("FullName", userInfo.FullName));

            return claims;
        }
    }
}
