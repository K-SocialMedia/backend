using Microsoft.AspNetCore.SignalR;
using ChatChit.Data;
using ChatChit.Models;

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

        public async Task JoinRoomChat(ChatRoomModel userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room + userConnection.User);
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.User + userConnection.Room);
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
