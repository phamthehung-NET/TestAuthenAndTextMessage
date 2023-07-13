﻿using Microsoft.AspNetCore.SignalR;

namespace TestAuthenAndTextMessage.Hubs
{
    public class SignalR : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
