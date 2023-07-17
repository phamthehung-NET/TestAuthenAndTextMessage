using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
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
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Constants.UserRole);
                return ErrorException.None;
            }
            return ErrorException.DoublicateUserName;
        }

        public async Task<UserDTO> GetUserInfo()
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await userManager.FindByIdAsync(userId);

            var roles = await userManager.GetRolesAsync(user);

            UserDTO currentUser = new()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessFailedCount = user.AccessFailedCount,
                ConcurrencyStamp = user.ConcurrencyStamp,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                NormalizedEmail = user.NormalizedEmail,
                NormalizedUserName = user.NormalizedUserName,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName,
                Role = roles.FirstOrDefault(),
            };

            return currentUser;
        }
    }
}
