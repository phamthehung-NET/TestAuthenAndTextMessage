using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Implementation
{
	public class MessageService : IMessageService
	{
		private readonly IMessageRepository repository;

        public MessageService(IMessageRepository _repository)
        {
			repository = _repository;
        }

        public async Task AddMessage(MessageDTO res)
		{
			var result = await repository.AddMessage(res);
			if (result == ErrorException.DatabaseError)
			{
				throw new Exception("Cannot add due to the error connection to Database");
			}
		}

		public async Task DeleteMessage(MessageDTO res)
		{
			var result = await repository.DeleteMessage(res);
			if (result == ErrorException.NotExist)
			{
				throw new Exception("This Message is not exist");
			}
			if (result == ErrorException.NotPermitted)
			{
				throw new Exception("You don't have permission to remove this message");
			}
		}

		public Pagination<MessageDTO> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize)
		{
			return repository.GetMessages(conversationId, belongToGroup, pageIndex, pageSize);
		}

		public void UpdateMessage(MessageDTO res)
		{
			var result = repository.UpdateMessage(res);
			if (result == ErrorException.NotExist)
			{
				throw new Exception("This Message is not exist");
			}
			if (result == ErrorException.NotPermitted)
			{
				throw new Exception("You don't have permission to edit this message");
			}
		}
	}
}
