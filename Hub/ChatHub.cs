using Microsoft.AspNetCore.SignalR;
using ChatChit.Data;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using System.Security.Claims;

namespace ChatChit.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatChitContex _context;
        
        public ChatHub(ChatChitContex context)
        {
            _context = context;
        }

        public async Task JoinRoom(ChatRoomModel userConnection)
        {

            //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", "Khoi", $"{userConnection.User} has joined {userConnection.Room}");
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
        }

        public async Task JoinRoomChat(string id)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{id}{userId}");
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}{id}");
            }
        }

        public async Task SendMessage(string message, string fromId, string toId, string Room)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.content = message;
            messageModel.createAt = DateTime.Now;
            messageModel.senderId = fromId;
            messageModel.receiverId = toId;
            messageModel.isRead = false;
            //_context.MessageMxhs.Add(mes);
            //await _context.SaveChangesAsync();

            await Clients.OthersInGroup(Room).SendAsync("ReceiveMessage", messageModel.content, messageModel.senderId);
        }
    }
}
