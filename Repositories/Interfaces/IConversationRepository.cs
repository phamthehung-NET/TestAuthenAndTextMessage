using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        Pagination<object> GetAllConversation(int pageIndex, int pageSize);

        ErrorException CreateConversation(string userId, string message);

        ErrorException DeleteConversation(int id);

        ErrorException CreateGroupChat(GroupDTO res);

        ErrorException DeleteGroupChat(int id);

        ErrorException UpdateGroupChat(GroupDTO group);
    }
}
