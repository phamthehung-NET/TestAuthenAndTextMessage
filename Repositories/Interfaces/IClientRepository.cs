using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<List<ClientDTO>> GetClientsInConversation(int conversationId, bool belongToGroup);

        Task<ErrorException> CreateWsClient(string connectionId, string currentUserId);

        Task<ErrorException> DeleteWsClient(string connectionId);
    }
}
