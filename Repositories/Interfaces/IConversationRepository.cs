using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        Task<ResponseModel> GetAllConversation(int pageIndex, int pageSize);

        ErrorException CreateConversation(string userId, string message);

        ErrorException DeleteConversation(int id);

        ErrorException CreateGroupChat(GroupDTO res);

        ErrorException DeleteGroupChat(int id);

        ErrorException UpdateGroupChat(GroupDTO group);

        Task<ResponseModel> SearchUser(string keyword);
	}
}
