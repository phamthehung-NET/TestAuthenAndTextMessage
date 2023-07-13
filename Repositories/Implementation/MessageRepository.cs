using System.Security.Claims;
using TestAuthenAndTextMessage.Data;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Implementation
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MessageRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
        }

        public ErrorException AddMessage(MessageDTO res)
        {
            var currentUser = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Message message = new()
            {
                Content = res.Content,
                ConversationId = res.ConversationId,
                BelongToGroup = res.BelongToGroup,
                AuthorId = currentUser,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            context.Add(message);
            context.SaveChanges();
            return message.Id > 0 ? ErrorException.None : ErrorException.DatabaseError;
        }

        public ErrorException DeleteMessage(MessageDTO res)
        {
            var currentUser = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var message = context.Messages.FirstOrDefault(x => x.Id == res.Id);
            if (message != null)
            {
                if (message.AuthorId.Equals(currentUser))
                {
                    message.IsSelfDelete = res.IsSelfDelete;
                    message.IsDeleteForEveryOne = res.IsDeleteForEveryOne;
                    context.SaveChanges();
                    return ErrorException.None;
                }
                return ErrorException.NotPermitted;
            }
            return ErrorException.NotExist;
        }

        public Pagination<MessageDTO> GetMessages(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public ErrorException UpdateMessage(MessageDTO res)
        {
            throw new NotImplementedException();
        }
    }
}
