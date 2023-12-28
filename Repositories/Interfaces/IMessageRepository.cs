using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<ResponseModel> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize);

        Task<ErrorException> AddMessage(MessageDTO res);

        Task<ErrorException> DeleteMessage(MessageDTO res);

        ErrorException UpdateMessage(MessageDTO res);
    }
}
