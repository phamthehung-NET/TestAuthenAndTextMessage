using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Pagination<MessageDTO> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize);

        Task<ErrorException> AddMessage(MessageDTO res);

        Task<ErrorException> DeleteMessage(MessageDTO res);

        ErrorException UpdateMessage(MessageDTO res);
    }
}
