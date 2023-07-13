using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IConversationService
    {
        Pagination<object> GetAllConversation(int pageIndex, int pageSize);

        void CreateConversation(string userId, string message);

        void DeleteConversation(int id);

        void CreateGroupChat(GroupDTO res);

        void DeleteGroupChat(int id);

        void UpdateGroupChat(GroupDTO group);
    }
}
