using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Services.Implementation
{
	public class ClientService : IClientService
	{
		private readonly IClientRepository repository;

        public ClientService(IClientRepository _repository)
        {
			repository = _repository;
        }

        public async Task CreateWsClient(string connectionId, string currentUserId)
		{
			var result = await repository.CreateWsClient(connectionId, currentUserId);
			if(result == ErrorException.DatabaseError)
			{
				throw new Exception("Cannot connect to the database");
			}
		}

		public async Task DeleteWsClient(string connectionId)
		{
			var result = await repository.DeleteWsClient(connectionId);
			if(result == ErrorException.NotExist)
			{
				throw new Exception("Client is not exist");
			}
		}

		public async Task<List<ClientDTO>> GetClientsInConversation(int conversationId, bool belongToGroup)
		{
			return await repository.GetClientsInConversation(conversationId, belongToGroup);
		}
	}
}
