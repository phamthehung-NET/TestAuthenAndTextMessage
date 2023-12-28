using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IMessageService
    {
		Task<ResponseModel> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize);

		Task<ResponseModel> AddMessage(MessageDTO res);

		Task<ResponseModel> DeleteMessage(MessageDTO res);

		ResponseModel UpdateMessage(MessageDTO res);
	}
}
