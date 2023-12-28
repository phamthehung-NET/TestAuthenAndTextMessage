using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IAttachmentRepository
    {
        Pagination<Attachment> GetAllAttachment(int conversationId, bool belongToGroup, int pageIndex, int pageSize);

        ErrorException AddAttachment(Attachment attachment);

        ErrorException RemoveAttachment(string attachmentLink, int attachmentType);
    }
}
