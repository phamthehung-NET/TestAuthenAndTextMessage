using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Services.Interfaces;

namespace TestAuthenAndTextMessage.Hubs
{
    [Authorize]
    public class SignalR : Hub
    {
        private readonly IClientService service;

        public SignalR(IClientService _service)
        {
            service = _service;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await service.CreateWsClient(Context.ConnectionId, currentUserId);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
				await service.DeleteWsClient(Context.ConnectionId);
				await base.OnDisconnectedAsync(exception);
			}
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Handle show message on client "{user} is entering..."
        public async Task OnUserInput(int conversationId, bool belongtoGroup)
        {
            try
            {
                var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                List<ClientDTO> clients = await service.GetClientsInConversation(conversationId, belongtoGroup);
                
                var currentClientLastName = clients.FirstOrDefault(x => x.UserId.Equals(currentUserId)).UserLastName;

                IReadOnlyList<string> connectionIds = clients.Where(x => !x.UserId.Equals(currentUserId)).Select(x => x.SignalRClientId).ToList();

                await Clients.Clients(connectionIds).SendAsync("OnUserInput", currentClientLastName, conversationId, belongtoGroup);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

		//Handle remove message "{user} is entering..." on client
		public async Task OnUserStopInput(int conversationId, bool belongtoGroup)
		{
			try
			{
				var currentUserId = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

				List<ClientDTO> clients = await service.GetClientsInConversation(conversationId, belongtoGroup);

				var currentClientLastName = clients.FirstOrDefault(x => x.UserId.Equals(currentUserId)).UserLastName;

				IReadOnlyList<string> connectionIds = clients.Where(x => !x.UserId.Equals(currentUserId)).Select(x => x.SignalRClientId).ToList();

				await Clients.Clients(connectionIds).SendAsync("OnUserStopInput", currentClientLastName, conversationId, belongtoGroup);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
