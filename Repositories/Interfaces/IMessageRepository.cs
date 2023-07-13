using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Pagination<MessageDTO> GetMessages(int pageIndex, int pageSize);

        ErrorException AddMessage(MessageDTO res);

        ErrorException DeleteMessage(int id);

        ErrorException UpdateMessage(MessageDTO res);
    }
}
