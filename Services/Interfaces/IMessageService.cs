using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IMessageService
    {
		Pagination<MessageDTO> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize);

		Task AddMessage(MessageDTO res);

		Task DeleteMessage(MessageDTO res);

		void UpdateMessage(MessageDTO res);
	}
}
