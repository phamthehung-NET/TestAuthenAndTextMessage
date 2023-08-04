using Accounting.Utilities;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TestAuthenAndTextMessage.Data;
using TestAuthenAndTextMessage.Hubs;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Implementation
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAttachmentService attachmentService;
        private readonly IClientService clientService;
        private readonly IHubContext<SignalR> hubContext;

        public MessageRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, IAttachmentService _attachmentService, IHubContext<SignalR> _hubContext, IClientService _clientService)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
            attachmentService = _attachmentService;
            hubContext = _hubContext;
            clientService = _clientService;
        }

        public async Task<ErrorException> AddMessage(MessageDTO res)
        {
            var currentUser = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Message message = new()
            {
                Content = (MessageType)res.MessageType == MessageType.Text || (MessageType)res.MessageType == MessageType.Link ? res.Content : HelperFunctions.UploadBase64File(res.Content, res.FileName, Constants.AttachmentLink),
                ConversationId = res.ConversationId,
                BelongToGroup = res.BelongToGroup,
                AuthorId = currentUser,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                MessageType = res.MessageType,
            };
            context.Add(message);
            context.SaveChanges();

            if((MessageType)res.MessageType == MessageType.File || (MessageType)res.MessageType == MessageType.Media)
            {
                attachmentService.AddAttachment(new()
                {
                    AttachmentLink = message.Content,
                    AttachmentType = message.MessageType,
                    AuthorId = currentUser,
                    BelongToGroup = message.BelongToGroup,
                    ConversationId = message.ConversationId,
                });
            }

            if(message.Id > 0)
            {
				await SendMessageToClient(message);
			}

            return message.Id > 0 ? ErrorException.None : ErrorException.DatabaseError;
        }

        public async Task<ErrorException> DeleteMessage(MessageDTO res)
        {
            var currentUser = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var message = context.Messages.FirstOrDefault(x => x.Id == res.Id);
            if (message != null)
            {
                if (message.AuthorId.Equals(currentUser))
                {
                    message.IsSelfDelete = res.IsSelfDelete;
                    message.IsDeleteForEveryOne = res.IsDeleteForEveryOne;
                    message.ModifiedDate = DateTime.Now;
                    context.SaveChanges();

					if ((MessageType)res.MessageType == MessageType.File || (MessageType)res.MessageType == MessageType.Media)
					{
						attachmentService.RemoveAttachment(message.Content, message.MessageType);
					}
                    
                    await SendMessageToClient(message);

					return ErrorException.None;
                }
                return ErrorException.NotPermitted;
            }
            return ErrorException.NotExist;
        }

        public Pagination<MessageDTO> GetMessages(int conversationId, bool belongToGroup, int pageIndex, int pageSize)
        {
            return (from m in context.Messages
                          join u in context.Users on m.AuthorId equals u.Id
                          where m.ConversationId == conversationId && m.BelongToGroup == belongToGroup
                          select new MessageDTO
                          {
                              Id = m.Id,
                              Content = m.Content,
                              BelongToGroup = m.BelongToGroup,
                              ConversationId = m.ConversationId,
                              CreatedDate = m.CreatedDate,
                              ModifiedDate = m.ModifiedDate,
                              IsDeleteForEveryOne = m.IsDeleteForEveryOne,
                              IsSelfDelete = m.IsSelfDelete,
                              MessageType = m.MessageType,
                              Author = new()
                              {
                                  Id = m.AuthorId,
                                  FirstName = u.FirstName,
                                  LastName = u.LastName,
                                  Avatar = u.Avatar,
                              },
                          }).OrderByDescending(x => x.CreatedDate).Paginate(pageIndex, pageSize);
        }

        public ErrorException UpdateMessage(MessageDTO res)
        {
			var currentUser = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var message = context.Messages.FirstOrDefault(x => x.Id == res.Id);
			if (message != null)
			{
				if (message.AuthorId.Equals(currentUser))
				{
                    message.Content = (MessageType)res.MessageType == MessageType.Text || (MessageType)res.MessageType == MessageType.Link ? res.Content : HelperFunctions.UploadBase64File(res.Content, res.FileName, Constants.AttachmentLink);
                    message.MessageType = res.MessageType;
					message.ModifiedDate = DateTime.Now;
					context.SaveChanges();
					return ErrorException.None;
				}
				return ErrorException.NotPermitted;
			}
			return ErrorException.NotExist;
		}

        private async Task SendMessageToClient(Message msg)
        {
			var currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

			List<ClientDTO> clients = await clientService.GetClientsInConversation(msg.ConversationId, msg.BelongToGroup);

			IReadOnlyList<string> connectionIds = clients.Select(x => x.SignalRClientId).ToList();

			await hubContext.Clients.Clients(connectionIds).SendAsync("OnMessageSent", msg);
		}
	}
}
