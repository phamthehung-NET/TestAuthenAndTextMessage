using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAuthenAndTextMessage.Services.Interfaces;

namespace TestAuthenAndTextMessage.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService service;

        public ConversationController(IConversationService _service)
        {
            service = _service;
        }

        [HttpGet]
        public IActionResult GetAllConversation(int? pageIndex, int? pageSize)
        {
            pageIndex ??= 1;
            pageSize ??= 10;

            try
            {
                return Ok(service.GetAllConversation(pageIndex.Value, pageSize.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ContactToUser(string userId, string message)
        {
            try
            {
                service.CreateConversation(userId, message);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteConversation(int id)
        {
            try
            {
                service.DeleteConversation(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
