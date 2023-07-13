using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Implementation
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationRepository(UserManager<CustomUser> _userManager, IConfiguration _configuration, IHttpContextAccessor _httpContextAccessor)
        {
            userManager = _userManager;
            configuration = _configuration;
            httpContextAccessor = _httpContextAccessor;
        }

        public async Task<object> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expration = token.ValidTo
                };
            }
            return null;
        }

        public async Task<ErrorException> Resgiter(RegisterModel model)
        {
            CustomUser user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return ErrorException.None;
            }
            return ErrorException.DoublicateUserName;
        }

        public async Task<CustomUser> GetUserInfo()
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentUser = await userManager.FindByIdAsync(userId);

            return currentUser;
        }
    }
}
