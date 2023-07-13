using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Services.Interfaces;

namespace TestAuthenAndTextMessage.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService service;

        public AuthenticationController(IAuthenticationService _service)
        {
            service = _service;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    return Ok(await service.Login(model));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Resgiter(RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await service.Resgiter(model);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            return Ok(await service.GetUserInfo());
        }
    }
}
