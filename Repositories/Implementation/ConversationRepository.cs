using Accounting.Utilities;
using System.Security.Claims;
using TestAuthenAndTextMessage.Data;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Implementation
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMessageService messageService;

        public ConversationRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, IMessageService _messageService)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
            messageService = _messageService;
        }

        public ErrorException CreateConversation(string userId, string message)
        {
            var currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var conversationDb = context.Conversations.FirstOrDefault(x => (x.User1Id.Equals(userId) && x.User2Id.Equals(currentUserId)) || (x.User1Id.Equals(currentUserId) && x.User2Id.Equals(userId)));

            if (conversationDb == null)
            {
                var user2 = context.Users.FirstOrDefault(x => x.Id.Equals(userId));
                if (user2 != null)
                {
                    Conversation conversation = new()
                    {
                        User1Id = currentUserId,
                        User2Id = userId,
                        CreatedDate = DateTime.Now,
                    };

                    context.Add(conversation);
                    context.SaveChanges();

                    messageService.AddMessage(new MessageDTO
                    {
                        AuthorId = currentUserId,
                        ConversationId = conversation.Id,
                        Content = message,
                        BelongToGroup = false,
                        MessageType = (int)MessageType.Text,
                    });

                    return ErrorException.None;
                }
                return ErrorException.NotExist;
            }
            else
            {
				messageService.AddMessage(new MessageDTO
				{
					AuthorId = currentUserId,
					ConversationId = conversationDb.Id,
					Content = message,
					BelongToGroup = false,
					MessageType = (int)MessageType.Text,
				});
				return ErrorException.None;
            }
        }

        public ErrorException DeleteConversation(int id)
        {
            var currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var conversation = context.Conversations.FirstOrDefault(c => c.Id == id);

            if (conversation != null)
            {
                if (currentUserId.Equals(conversation.User1Id))
                {
                    conversation.IsUser1Deleted = true;
                }
                else
                {
                    conversation.IsUser2Deleted = true;
                }
                context.SaveChanges();
                return ErrorException.None;
            }
            return ErrorException.NotExist;
        }

        public Pagination<object> GetAllConversation(int pageIndex, int pageSize)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<dynamic> conversationsAndGroups = new();

            var conversation = (from c in context.Conversations
                                join u1 in context.Users on c.User1Id equals u1.Id
                                join u2 in context.Users on c.User2Id equals u2.Id
                                join m in context.Messages on c.Id equals m.ConversationId into msgs
                                from m in msgs.DefaultIfEmpty()
                                join mu in context.Users on m.AuthorId equals mu.Id into msgusers
                                from mu in msgusers.DefaultIfEmpty()
                                where (u1.Id.Equals(userId) || u2.Id.Equals(userId))
                                select new
                                {
                                    c.Id,
                                    User1Id = u1.Id,
                                    User1FirstName = u1.FirstName,
                                    User1LastName = u1.LastName,
                                    User2Id = u2.Id,
									User2FirstName = u2.FirstName,
									User2LastName = u2.LastName,
									User1Avatar = u1.Avatar,
									User2Avatar = u2.Avatar,
									c.IsUser1Deleted,
                                    c.IsUser2Deleted,
                                    c.CreatedDate,
                                    c.ModifiedDate,
                                    LastestMessageId = m.Id,
                                    LastestMessageContent = m.Content,
                                    LastestMessageCreatedDate = m.CreatedDate,
                                    LastestMessageModifiedDate = m.ModifiedDate,
                                    LastestMessageAuthorFirstName = mu.FirstName,
                                    LastestMessageAuthorLastName = mu.LastName,
                                    LastestMessageAuthorId = mu.Id,
                                    LastestMessageBelongToGroup = m.BelongToGroup,
                                }).GroupBy(x => new { x.Id, x.User1FirstName, x.User1LastName, x.User1Avatar, x.User1Id, x.User2Id, x.User2FirstName, x.User2LastName, x.User2Avatar, x.IsUser1Deleted, x.IsUser2Deleted, x.CreatedDate, x.ModifiedDate })
                               .Select(x => new ConversationDTO
                               {
                                   Id = x.Key.Id,
                                   User1Id = x.Key.User1Id,
                                   User2Id = x.Key.User2Id,
                                   User1FirstName = x.Key.User1FirstName,
                                   User1LastName = x.Key.User1LastName,
                                   User1Avatar = x.Key.User1Avatar,
                                   User2FirstName = x.Key.User2FirstName,
                                   User2LastName = x.Key.User2LastName,
                                   User2Avatar = x.Key.User2Avatar,
                                   IsUser1Deleted = x.Key.IsUser1Deleted,
                                   IsUser2Deleted = x.Key.IsUser2Deleted,
                                   CreatedDate = x.Key.CreatedDate,
                                   ModifiedDate = x.Key.ModifiedDate,
                                   LastestMessage = x.Where(y => !y.LastestMessageBelongToGroup).Select(y => new MessageDTO
                                   {
                                       Id = y.LastestMessageId,
                                       Content = y.LastestMessageContent,
                                       ConversationId = x.Key.Id,
                                       BelongToGroup = y.LastestMessageBelongToGroup,
                                       CreatedDate = y.CreatedDate,
                                       ModifiedDate = y.ModifiedDate,
                                       AuthorId = y.LastestMessageAuthorId,
                                       AuthorFirstName = y.LastestMessageAuthorFirstName,
                                       AuthorLastName = y.LastestMessageAuthorLastName,
                                   }).Where(y => y.Id > 0).OrderByDescending(x => x.CreatedDate).FirstOrDefault(),
                               });
            var groups = GetAllGroups(userId);

            conversationsAndGroups.AddRange(conversation);
            conversationsAndGroups.AddRange(groups);

            return conversationsAndGroups.Where(x => x.LastestMessage != null).AsQueryable().Paginate(pageIndex, pageSize);
        }

        public ErrorException CreateGroupChat(GroupDTO res)
        {
            var currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Group group = new()
            {
                AdminId = currentUserId,
                Name = res.Name,
                Avatar = HelperFunctions.UploadBase64File(res.Avatar, res.AvatarFileName, Constants.GroupAvatarDirectory),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            context.Groups.Add(group);
            context.SaveChanges();
			
            messageService.AddMessage(new MessageDTO
			{
				AuthorId = currentUserId,
				ConversationId = group.Id,
				Content = res.Message,
				BelongToGroup = true,
				MessageType = (int)MessageType.Text,
			});

            res.MemberIds.ForEach(x =>
            {
                UserGroupChat userGroup = new()
                {
                    GroupId = group.Id,
                    UserId = x
                };
                context.Add(userGroup);
            });
            context.SaveChanges();

			return group.Id > 0 ? ErrorException.None : ErrorException.DatabaseError;
        }

        public ErrorException DeleteGroupChat(int id)
        {
            var currentUserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var group = context.Groups.FirstOrDefault(x => x.Id == id);
            if (group != null)
            {
                if (group.AdminId == currentUserId)
                {
                    var message = context.Messages.Where(x => x.BelongToGroup && x.ConversationId == id);
                    context.Groups.Remove(group);

                    context.RemoveRange(message);
                    context.SaveChanges();

                    return ErrorException.None;
                }
                return ErrorException.NotPermitted;
            }
            return ErrorException.NotExist;
        }

        private IQueryable<GroupDTO> GetAllGroups(string currentUserId)
        {
            return (from g in context.Groups
                    join ug in context.UserGroupsChats on g.Id equals ug.GroupId
                    join u in context.Users on g.AdminId equals u.Id
                    join m in context.Messages on g.Id equals m.ConversationId into mgs
                    from m in mgs.DefaultIfEmpty()
                    join mu in context.Users on m.AuthorId equals mu.Id into msgusers
					from mu in msgusers.DefaultIfEmpty()
					where ug.UserId.Equals(currentUserId)
                    select new
                    {
                        g.Id,
                        g.Name,
                        g.AdminId,
                        g.Avatar,
                        g.CreatedDate,
                        g.ModifiedDate,
                        LastestMessageId = m.Id,
                        LastestMessageContent = m.Content,
                        LastestMessageCreatedDate = m.CreatedDate,
                        LastestMessageModifiedDate = m.ModifiedDate,
						LastestMessageAuthorFirstName = mu.FirstName,
						LastestMessageAuthorLastName = mu.LastName,
						LastestMessageAuthorId = mu.Id,
                        LastestMessageBelongToGroup = m.BelongToGroup,
					}).GroupBy(x => new { x.Id, x.Name, x.AdminId, x.Avatar, x.CreatedDate, x.ModifiedDate })
                         .Select(x => new GroupDTO
                         {
                             Id = x.Key.Id,
                             Name = x.Key.Name,
                             AdminId = x.Key.AdminId,
                             Avatar = x.Key.Avatar,
                             CreatedDate = x.Key.CreatedDate,
                             ModifiedDate = x.Key.ModifiedDate,
                             LastestMessage = x.Where(y => y.LastestMessageBelongToGroup).Select(y => new MessageDTO
                             {
                                 Id = y.LastestMessageId,
                                 Content = y.LastestMessageContent,
                                 ConversationId = x.Key.Id,
                                 BelongToGroup = false,
                                 CreatedDate = y.CreatedDate,
                                 ModifiedDate = y.ModifiedDate,
                                 AuthorId = y.LastestMessageAuthorId,
								 AuthorFirstName = y.LastestMessageAuthorFirstName,
								 AuthorLastName = y.LastestMessageAuthorLastName,
							 }).Where(y => y.Id > 0).OrderByDescending(x => x.CreatedDate).FirstOrDefault(),
                         });
        }

        public ErrorException UpdateGroupChat(GroupDTO group)
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var groupChat = context.Groups.FirstOrDefault(x => x.Id == group.Id);
            if (groupChat != null)
            {
                if (userId.Equals(groupChat.AdminId))
                {
                    groupChat.Avatar = HelperFunctions.UploadBase64File(group.Avatar, group.AvatarFileName, Constants.GroupAvatarDirectory);
                    groupChat.ModifiedDate = DateTime.Now;
                    groupChat.Name = group.Name;

                    context.SaveChanges();
                    return ErrorException.None;
                }
                return ErrorException.NotPermitted;
            }
            return ErrorException.NotExist;
        }

        public IQueryable<CustomUser> SearchUser(string keyword)
        {
            return !string.IsNullOrEmpty(keyword) ? context.Users
                .Where(x => x.FirstName.ToLower().Contains(keyword.ToLower())
                || x.LastName.ToLower().Contains(keyword.ToLower())
                || x.UserName.ToLower().Contains(keyword.ToLower())
                || x.PhoneNumber.ToLower().Contains(keyword.ToLower())
                || x.Email.ToLower().Contains(keyword.ToLower())) : null;
		}
    }
}
