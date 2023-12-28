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
	public class MessageController : ControllerBase
	{
		private readonly IMessageService service;

        public MessageController(IMessageService _service)
        {
            service = _service;
        }

		[HttpGet]
		public async Task<IActionResult> GetAllMessages(int conversationId, bool belongToGroup, int? pageIndex, int? pageSize)
		{
			ResponseModel res = new();
			pageIndex ??= 1;
			pageSize ??= 10;

			try
			{
				res = await service.GetMessages(conversationId, belongToGroup, pageIndex.Value, pageSize.Value);

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
		public async Task<IActionResult> CreateMessage(MessageDTO req)
		{
			ResponseModel res = new();
			try
			{
				 res = await service.AddMessage(req);
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
		public IActionResult UpdateMessage(MessageDTO req)
		{
			ResponseModel res = new();
            try
            {
                res = service.UpdateMessage(req);
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
		public async Task<IActionResult> DeleteMessage(MessageDTO req)
		{
			ResponseModel res = new();
            try
            {
                res = await service.DeleteMessage(req);
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
