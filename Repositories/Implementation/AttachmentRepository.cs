using Accounting.Utilities;
using System.Security.Claims;
using TestAuthenAndTextMessage.Data;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Repositories.Implementation
{
	public class AttachmentRepository : IAttachmentRepository
	{
		private readonly ApplicationDbContext context;
		private readonly IHttpContextAccessor httpContextAccessor;

        public AttachmentRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor)
        {
			context = _context;
			httpContextAccessor = _httpContextAccessor;
        }

        public ErrorException AddAttachment(Attachment attachment)
		{
			attachment.CreatedDate = DateTime.Now;
			attachment.ModifiedDate = DateTime.Now;
			context.Attachments.Add(attachment);
			context.SaveChanges();
			return attachment.Id > 0 ? ErrorException.None : ErrorException.DatabaseError;
		}

		public Pagination<Attachment> GetAllAttachment(int conversationId, bool belongToGroup, int pageIndex, int pageSize)
		{
			return context.Attachments.Where(x => x.ConversationId == conversationId && x.BelongToGroup == belongToGroup).OrderByDescending(x => x.CreatedDate).Paginate(pageIndex, pageSize);
		}

		public ErrorException RemoveAttachment(string attachmentLink, int attachmentType)
		{
			var currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var attachment = GetAttachment(attachmentLink, attachmentType);
			if (attachment != null)
			{
				if(currentUserId == attachment.AuthorId)
				{
					context.Attachments.Remove(attachment);
					context.SaveChanges();
					HelperFunctions.RemoveFile(attachmentLink);
					return ErrorException.None;
				}
				return ErrorException.NotPermitted;
			}
			return ErrorException.NotExist;
		}

		private Attachment GetAttachment(string attachmentLink, int attachmentType)
		{
			return context.Attachments.FirstOrDefault(x => x.AttachmentLink.Equals(attachmentLink) && x.AttachmentType == attachmentType);
		}
	}
}
