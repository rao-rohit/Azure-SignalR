using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.SignalRHub
{
    public class NotificationHub :Hub
    {
            protected IHubContext<NotificationHub> _context;
            public NotificationHub(IHubContext<NotificationHub> context)
            {
                this._context = context;
            }


            public async Task ReceiveMessage(string message)
            {
                await _context.Clients.All.SendAsync("ReceiveMessage", message);
            }
        
        
    }
}
