using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Implementation
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository repository;

        public ConversationService(IConversationRepository _repository)
        {
            repository = _repository;
        }

        public void CreateConversation(string userId, string message)
        {
            var result = repository.CreateConversation(userId, message);
            if(result == ErrorException.NotExist)
            {
                throw new Exception("This User is Not Exist");
            }
        }

        public void CreateGroupChat(GroupDTO res)
        {
            repository.CreateGroupChat(res);
        }

        public void DeleteConversation(int id)
        {
            var result = repository.DeleteConversation(id);
            if (result == ErrorException.NotExist)
            {
                throw new Exception("This Conversation is Not Exist");
            }
        }

        public void DeleteGroupChat(int id)
        {
            var result = repository.DeleteGroupChat(id);
            if (result == ErrorException.NotExist)
            {
                throw new Exception("This Group chat is not exist");
            }
            if (result == ErrorException.NotPermitted)
            {
                throw new Exception("You need Admin permission to delete this group");
            }
        }

        public Pagination<object> GetAllConversation(int pageIndex, int pageSize)
        {
            return repository.GetAllConversation(pageIndex, pageSize);
        }

        public void UpdateGroupChat(GroupDTO group)
        {
            var result = repository.UpdateGroupChat(group);
            if (result == ErrorException.NotExist)
            {
                throw new Exception("This Group chat is not exist");
            }
            if (result == ErrorException.NotPermitted)
            {
                throw new Exception("You need Admin permission to update this group");
            }
        }

		public IQueryable<CustomUser> SearchUser(string keyword)
        {
            return repository.SearchUser(keyword);
        }
	}
}
