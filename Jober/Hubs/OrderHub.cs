using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jober.Hubs
{
    public class OrderHub : Hub
    {
        public async Task Send(string message)
        {
            await this.Clients.AllExcept(new string[] { this.Context.ConnectionId }).InvokeAsync("Send", message);
        }
    }
}
