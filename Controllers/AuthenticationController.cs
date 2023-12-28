using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAuthenAndTextMessage.Extensions;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [ValidationFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService service;
        private readonly IConfiguration config;

        public AuthenticationController(IAuthenticationService _service, IConfiguration _config)
        {
            service = _service;
            config = _config;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            ResponseModel res = new();
            try
            {
                res = await service.Login(model);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Error = true;
                res.Message = ex.Message;
                return Unauthorized(res);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            try
            {
                await service.Resgiter(model);
                return Ok();
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
            var res = await service.GetUserInfo();
            return Ok(res);
        }
	}
}
