using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Web.WebPages.Html;
using test.Data;
using test.Models;

namespace test.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DepiContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ChatHub(DepiContext context,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public  override Task OnConnectedAsync()
        {
            var userId = _userManager.GetUserId(Context.User);
            if (userId == null)
            {
                return base.OnConnectedAsync();
            }
            var con = new UserConnections
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId
            };
            _context.UserConnections.Add(con);
            _context.SaveChanges();

            return  base.OnConnectedAsync();
        }

        public async Task sendmessage(ChatMessage chatMessage)
        {
            if (chatMessage != null)
            {
                chatMessage.read = 0;
                chatMessage.Time= DateTime.Now;
                _context.ChatMessages.Add(chatMessage);
                await _context.SaveChangesAsync();
            }
            var connectionsid = _context.UserConnections
    .Where(c => c.UserId == chatMessage.SenderId)
    .Select(p => p.ConnectionId)
    .ToList();
    
var connectionsid2 = _context.UserConnections
    .Where(c => c.UserId == chatMessage.ReceiverId)
    .Select(p => p.ConnectionId)
    .ToList(); 

            if (connectionsid != null) {
                foreach (var connection in connectionsid) {

                 await   Clients.Client(connection).SendAsync("sendermessage",chatMessage);
                        }
                foreach (var connection in connectionsid2)
                {

                    await Clients.Client(connection).SendAsync("recievermessage", chatMessage);
                    await Clients.Client(connection).SendAsync("updateNotifications", chatMessage.SenderId);
                }
                    }
            
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = _context.UserConnections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (connection != null)
            {
                _context.UserConnections.Remove(connection);
                _context.SaveChanges();
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
