using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityMiniinstagram.Web.Hubs
{
    public class CommentaryHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("SendCommentHub", message);
        }
    }
}
