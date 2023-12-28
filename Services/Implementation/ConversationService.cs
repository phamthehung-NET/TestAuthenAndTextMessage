using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Services.Implementation
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository repository;

        public ConversationService(IConversationRepository _repository)
        {
            repository = _repository;
        }

        public ResponseModel CreateConversation(string userId, string message)
        {
            ResponseModel res = new();
            var result = repository.CreateConversation(userId, message);
            
            if(result == ErrorException.NotExist)
            {
                throw new Exception("This User is Not Exist");
            }

            res.Message = "Success";
            return res;
        }

        public ResponseModel CreateGroupChat(GroupDTO req)
        {
            ResponseModel res = new();
            var result = repository.CreateGroupChat(req);

            if (result == ErrorException.None)
            {
                res.Message = "Success";
            }
            else
            {
                throw new Exception("Database error");
            }

            return res;
        }

        public ResponseModel DeleteConversation(int id)
        {
            ResponseModel res = new();
            var result = repository.DeleteConversation(id);
            
            if (result == ErrorException.NotExist)
            {
                throw new Exception("This Conversation is Not Exist");
            }

            res.Message = "Success";
            return res;
        }

        public ResponseModel DeleteGroupChat(int id)
        {
            ResponseModel res = new();
            var result = repository.DeleteGroupChat(id);
            
            if (result == ErrorException.NotExist)
            {
                throw new Exception("This Group chat is not exist");
            }
            if (result == ErrorException.NotPermitted)
            {
                throw new Exception("You need Admin permission to delete this group");
            }

            res.Message = "Success";
            return res;
        }

        public async Task<ResponseModel> GetAllConversation(int pageIndex, int pageSize)
        {
            return await repository.GetAllConversation(pageIndex, pageSize);
        }

        public ResponseModel UpdateGroupChat(GroupDTO group)
        {
            ResponseModel res = new();
            var result = repository.UpdateGroupChat(group);
            
            if (result == ErrorException.NotExist)
            {
                throw new Exception("This Group chat is not exist");
            }
            if (result == ErrorException.NotPermitted)
            {
                throw new Exception("You need Admin permission to update this group");
            }

            res.Message = "Success";
            return res;
        }

		public async Task<ResponseModel> SearchUser(string keyword)
        {
            return await repository.SearchUser(keyword);
        }
	}
}
