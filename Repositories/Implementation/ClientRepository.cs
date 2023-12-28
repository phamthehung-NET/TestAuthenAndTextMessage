using Microsoft.EntityFrameworkCore;
using TestAuthenAndTextMessage.Data;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Repositories.Implementation
{
	public class ClientRepository : IClientRepository
	{
		private readonly ApplicationDbContext context;

        public ClientRepository(ApplicationDbContext _context)
        {
			context = _context;
        }

        public async Task<ErrorException> CreateWsClient(string connectionId, string currentUserId)
		{
			var connectionDb = context.Clients.FirstOrDefault(x => x.UserId.Equals(currentUserId));
			if(connectionDb == null)
			{
				Client client = new()
				{
					UserId = currentUserId,
					SignalRClientId = connectionId,
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now,
				};
				await context.AddAsync(client);
				await context.SaveChangesAsync();
				return client.Id > 0 ? ErrorException.None : ErrorException.DatabaseError;
			}
			else
			{
				connectionDb.SignalRClientId = connectionId;
				await context.SaveChangesAsync();
				return ErrorException.None;
			}
		}

		public async Task<ErrorException> DeleteWsClient(string connectionId)
		{
			var client = context.Clients.FirstOrDefault(x => x.SignalRClientId.Equals(connectionId));
			if(client != null)
			{
				context.Remove(client);
				await context.SaveChangesAsync();
				return ErrorException.None;
			}
			return ErrorException.NotExist;
		}

		public async Task<List<ClientDTO>> GetClientsInConversation(int conversationId, bool belongToGroup)
		{
			List<ClientDTO> clients = new();
			if(belongToGroup)
			{
				clients = await (from g in context.Groups
								 join gc in context.UserGroupsChats on g.Id equals gc.GroupId
								 join c in context.Clients on gc.UserId equals c.UserId
								 join u in context.Users on c.UserId equals u.Id
								 select new ClientDTO
								 {
									 Id = c.Id,
									 UserId = u.Id,
									 SignalRClientId = c.SignalRClientId,
									 UserFirstName = u.FirstName,
									 UserLastName = u.LastName,
								 }).ToListAsync();
			}
			else
			{
				var client1 = await (from c in context.Conversations
							  join c1 in context.Clients on c.User1Id equals c1.UserId
							  join u in context.Users on c1.UserId equals u.Id
							  select new ClientDTO
							  {
								  Id = c1.Id,
								  UserId = u.Id,
								  SignalRClientId = c1.SignalRClientId,
								  UserFirstName = u.FirstName,
								  UserLastName = u.LastName,
							  }).FirstOrDefaultAsync();
				var client2 = await (from c in context.Conversations
									 join c2 in context.Clients on c.User2Id equals c2.UserId
									 join u in context.Users on c2.UserId equals u.Id
									 select new ClientDTO
									 {
										 Id = c2.Id,
										 UserId = u.Id,
										 SignalRClientId = c2.SignalRClientId,
										 UserFirstName = u.FirstName,
										 UserLastName = u.LastName,
									 }).FirstOrDefaultAsync();
				if (client1 != null)
				{
					clients.Add(client1);
				}
				if(client2 != null)
				{
					clients.Add(client2);
				}
			}
			return clients;
		}
	}
}
