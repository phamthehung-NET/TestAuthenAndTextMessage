using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Services.Implementation
{
	public class MessageService : IMessageService
	{
		private readonly IMessageRepository repository;

        public MessageService(IMessageRepository _repository)
        {
			repository = _repository;
        }

        public async Task<ResponseModel> AddMessage(MessageDTO req)
		{
			ResponseModel res = new();
			var result = await repository.AddMessage(req);
			
			if (result == ErrorException.DatabaseError)
			{
				throw new Exception("Cannot add due to the error connection to Database");
			}

			res.Message = "Success";
            return res;
		}

		public async Task<ResponseModel> DeleteMessage(MessageDTO req)
		{
			ResponseModel res = new();
			var result = await repository.DeleteMessage(req);
			
			if (result == ErrorException.NotExist)
			{
				throw new Exception("This Message is not exist");
			}
			if (result == ErrorException.NotPermitted)
			{
				throw new Exception("You don't have permission to remove this message");
			}

			res.Message = "Success";
			return res;
		}

		public async Task<ResponseModel> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize)
		{
			return await repository.GetMessages(conversationId, belongToGroup, pageIndex, pageSize);
		}

		public ResponseModel UpdateMessage(MessageDTO req)
		{
			ResponseModel res = new();
			var result = repository.UpdateMessage(req);

			if (result == ErrorException.NotExist)
			{
				throw new Exception("This Message is not exist");
			}
			if (result == ErrorException.NotPermitted)
			{
				throw new Exception("You don't have permission to edit this message");
			}

			res.Message = "Success";
            return res;
		}
	}
}
