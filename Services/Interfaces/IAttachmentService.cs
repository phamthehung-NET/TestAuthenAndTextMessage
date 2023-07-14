using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IAttachmentService
    {
		Pagination<Attachment> GetAllAttachment(int conversationId, bool belongToGroup, int pageIndex, int pageSize);

		void AddAttachment(Attachment attachment);

		void RemoveAttachment(string attachmentLink, int attachmentType);
	}
}
