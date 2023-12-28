using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAuthenAndTextMessage.Extensions;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Services.Interfaces;

namespace TestAuthenAndTextMessage.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    [ValidationFilter]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService service;

        public ConversationController(IConversationService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConversation(int? pageIndex, int? pageSize)
        {
            ResponseModel res = new();

            pageIndex ??= 1;
            pageSize ??= 10;

            try
            {
                res = await service.GetAllConversation(pageIndex.Value, pageSize.Value);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Error = true;
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }

        [HttpPost]
        public IActionResult ContactToUser(string userId, string message)
        {
            ResponseModel res = new();
            try
            {
                res = service.CreateConversation(userId, message);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Error = true;
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteConversation(int id)
        {
            ResponseModel res = new();
            try
            {
                res = service.DeleteConversation(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                res.Error = true;
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }

        [HttpPost]
        public IActionResult CreateGroupChat(GroupDTO req)
        {
            ResponseModel res = new();
			try
			{
				res = service.CreateGroupChat(req);
                return Ok(res);
			}
			catch (Exception ex)
			{
                res.Error = true;
                res.Message = ex.Message;
				return BadRequest(res);
			}
		}

        [HttpDelete("{id}")]
        public IActionResult DeleteGroupChat(int id)
        {
            ResponseModel res = new();
			try
			{
				res = service.DeleteGroupChat(id);
				return Ok(res);
			}
			catch (Exception ex)
			{
                res.Error = true;
                res.Message = ex.Message;
                return BadRequest(res);
            }
		}

        [HttpPost]
        public IActionResult UpdateGroupChat(GroupDTO req)
        {
            ResponseModel res = new();
			try
			{
				res = service.UpdateGroupChat(req);
				return Ok(res);
			}
			catch (Exception ex)
			{
                res.Error = true;
                res.Message = ex.Message;
                return BadRequest(res);
            }
		}

		[HttpGet]
		public async Task<IActionResult> SearchUser(string keyword)
		{
            ResponseModel res = new();
			try
			{
                res = await service.SearchUser(keyword);

                return Ok(res);
			}
			catch (Exception ex)
			{
                res.Error = true;
                res.Message = ex.Message;
				return BadRequest(res);
			}
		}
	}
}
