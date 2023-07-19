using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Services.Interfaces;

namespace TestAuthenAndTextMessage.Controllers
{
	[Route("[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class MessageController : ControllerBase
	{
		private readonly IMessageService service;

        public MessageController(IMessageService _service)
        {
            service = _service;
        }

		[HttpGet]
		public IActionResult GetAllMessages(int conversationId, bool belongToGroup, int? pageIndex, int? pageSize)
		{
			pageIndex ??= 1;
			pageSize ??= 10;

			try
			{
				return Ok(service.GetMessages(conversationId, belongToGroup, pageIndex.Value, pageSize.Value));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public IActionResult CreateMessage(MessageDTO res)
		{
			try
			{
				service.AddMessage(res);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public IActionResult UpdateMessage(MessageDTO res)
		{
			try
			{
				service.UpdateMessage(res);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public IActionResult DeleteMessage(MessageDTO res)
		{
			try
			{
				service.DeleteMessage(res);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }
}
