using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Utilities;

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

        public async Task<ResponseModel> Login(LoginModel model)
        {
            var res = new ResponseModel();
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    //new Claim(ClaimTypes.Name, user.UserName),
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

                res.Message = "Login success";
                res.Data = new JwtSecurityTokenHandler().WriteToken(token);

                return res;
            }

            res.Error = true;
            res.Message = "Login failed";
            return res;
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
            return ErrorException.DuplicateUserName;
        }

        public async Task<ResponseModel> GetUserInfo()
        {
            ResponseModel res = new();

            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var token = await httpContextAccessor.HttpContext.GetTokenAsync(Constants.AccessToken);

            var user = await userManager.FindByIdAsync(userId);

            var roles = await userManager.GetRolesAsync(user);

            UserDTO currentUser = new()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Role = roles.FirstOrDefault(),
            };

            res.Message = "Get current user successfully";
            res.Data = HelperFunctions.EncryptAES(token, currentUser);

            return res;
        }
    }
}
