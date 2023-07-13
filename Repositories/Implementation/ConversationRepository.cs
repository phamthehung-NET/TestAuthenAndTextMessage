using Accounting.Utilities;
using System.Security.Claims;
using TestAuthenAndTextMessage.Data;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Implementation
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ConversationRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
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

                    //add message

                    return ErrorException.None;
                }
                return ErrorException.NotExist;
            }
            else
            {
                //add message
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

            List<object> conversationsAndGroups = new();

            var conversation = (from c in context.Conversations
                               join u1 in context.Users on c.User1Id equals u1.Id
                               join u2 in context.Users on c.User2Id equals u2.Id
                               join m in context.Messages on c.Id equals m.ConversationId into msgs
                               from m in msgs.DefaultIfEmpty()
                               join mu in context.Users on m.AuthorId equals mu.Id
                               where (u1.Id.Equals(userId) || u2.Id.Equals(userId)) && !m.BelongToGroup
                               select new
                               {
                                   c.Id,
                                   User1Id = u1.Id,
                                   User1FullName = u1.FullName,
                                   User2Id = u2.Id,
                                   User2FullName = u2.FullName,
                                   c.IsUser1Deleted,
                                   c.IsUser2Deleted,
                                   c.CreatedDate,
                                   c.ModifiedDate,
                                   LastestMessageId = m.Id,
                                   LastestMessageContent = m.Content,
                                   LastestMessageCreatedDate = m.CreatedDate,
                                   LastestMessageModifiedDate = m.ModifiedDate,
                                   LastestMessageAuthorName = mu.FullName,
                                   LastestMessageAuthorId = mu.Id,
                               }).GroupBy(x => new { x.Id, x.User1FullName, x.User1Id, x.User2Id, x.User2FullName, x.IsUser1Deleted, x.IsUser2Deleted, x.CreatedDate, x.ModifiedDate })
                               .Select(x => new ConversationDTO
                               {
                                   Id = x.Key.Id,
                                   User1Id = x.Key.User1Id,
                                   User2Id = x.Key.User2Id,
                                   User1FullName = x.Key.User1FullName,
                                   User2FullName = x.Key.User2FullName,
                                   IsUser1Deleted = x.Key.IsUser1Deleted,
                                   IsUser2Deleted = x.Key.IsUser2Deleted,
                                   CreatedDate = x.Key.CreatedDate,
                                   ModifiedDate = x.Key.ModifiedDate,
                                   LastestMessage = x.Select(y => new MessageDTO
                                   {
                                       Id = y.LastestMessageId,
                                       Content = y.LastestMessageContent,
                                       ConversationId = x.Key.Id,
                                       BelongToGroup = false,
                                       CreatedDate = y.CreatedDate,
                                       ModifiedDate = y.ModifiedDate,
                                       AuthorId = y.LastestMessageAuthorId,
                                       AuthorName = y.LastestMessageAuthorName,
                                   }).Where(y => y.Id > 0).OrderByDescending(x => x.CreatedDate).FirstOrDefault(),
                               });
            var groups = GetAllGroups(userId);

            conversationsAndGroups.AddRange(conversation);
            conversationsAndGroups.Add(groups);

            return conversationsAndGroups.AsQueryable().Paginate(pageIndex, pageSize);
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
            return ErrorException.None;
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
                         join mu in context.Users on m.AuthorId equals mu.Id
                         where ug.UserId.Equals(currentUserId) && m.BelongToGroup
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
                             LastestMessageAuthorName = mu.FullName,
                             LastestMessageAuthorId = mu.Id,
                         }).GroupBy(x => new { x.Id, x.Name, x.AdminId, x.Avatar, x.CreatedDate, x.ModifiedDate })
                         .Select(x => new GroupDTO
                         {
                             Id = x.Key.Id,
                             Name = x.Key.Name,
                             AdminId = x.Key.AdminId,
                             Avatar = x.Key.Avatar,
                             CreatedDate = x.Key.CreatedDate,
                             ModifiedDate = x.Key.ModifiedDate,
                             LastestMessage = x.Select(y => new MessageDTO
                             {
                                 Id = y.LastestMessageId,
                                 Content = y.LastestMessageContent,
                                 ConversationId = x.Key.Id,
                                 BelongToGroup = false,
                                 CreatedDate = y.CreatedDate,
                                 ModifiedDate = y.ModifiedDate,
                                 AuthorId = y.LastestMessageAuthorId,
                                 AuthorName = y.LastestMessageAuthorName,
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
    }
}
