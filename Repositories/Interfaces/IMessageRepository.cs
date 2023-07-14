using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Pagination<MessageDTO> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize);

        ErrorException AddMessage(MessageDTO res);

        ErrorException DeleteMessage(MessageDTO res);

        ErrorException UpdateMessage(MessageDTO res);
    }
}
