using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAuthenAndTextMessage.Services.Interfaces;

namespace TestAuthenAndTextMessage.Controllers
{
	[Route("[controller]/[action]")]
	[ApiController]
	[Authorize]
	public class AttachmentController : ControllerBase
	{
		private readonly IAttachmentService service;

        public AttachmentController(IAttachmentService _service)
        {
            service = _service;
        }

		[HttpGet]
		public IActionResult GetAllAttachments(int conversationId, bool belongToGroup, int? pageIndex, int? pageSize)
		{
			pageIndex ??= 1;
			pageSize ??= 12;
			
			try
			{
				return Ok(service.GetAllAttachment(conversationId, belongToGroup, pageIndex.Value, pageSize.Value));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }
}
