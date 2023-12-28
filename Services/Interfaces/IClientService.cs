using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IClientService
    {
		Task<List<ClientDTO>> GetClientsInConversation(int conversationId, bool belongToGroup);

		Task CreateWsClient(string connectionId, string currentUserId);

		Task DeleteWsClient(string connectionId);
	}
}
