using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IConversationService
    {
        Task<ResponseModel> GetAllConversation(int pageIndex, int pageSize);

        ResponseModel CreateConversation(string userId, string message);

        ResponseModel DeleteConversation(int id);

        ResponseModel CreateGroupChat(GroupDTO req);

        ResponseModel DeleteGroupChat(int id);

        ResponseModel UpdateGroupChat(GroupDTO group);

		Task<ResponseModel> SearchUser(string keyword);
	}
}
