using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IMessageService
    {
		Pagination<MessageDTO> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize);

		void AddMessage(MessageDTO res);

		void DeleteMessage(MessageDTO res);

		void UpdateMessage(MessageDTO res);
	}
}
