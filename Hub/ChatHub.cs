using Microsoft.AspNetCore.SignalR;
using ChatChit.Data;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace ChatChit.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatChitContex _context;
        
        public ChatHub(ChatChitContex context)
        {
            _context = context;
        }

        public async Task JoinRoom(string roomName)
        {

            //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", "Khoi", $"{userConnection.User} has joined {userConnection.Room}");
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task JoinRoomChat( ChatRoomModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(model.tokenUserId) as JwtSecurityToken;

            if (token != null)
            {
                string userId = token.Claims.First(claim => claim.Type == "userId").Value;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}{model.friendId}");
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{model.friendId}{userId}");
            }
        }

        public async Task SendMessage(string message, ChatRoomModel model, string roomName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(model.tokenUserId) as JwtSecurityToken;

            if (token != null)
            {
                string userId = token.Claims.First(claim => claim.Type == "userId").Value;

                string newRoomName = roomName ?? $"{userId}{model.friendId}";

                MessageModel messageModel = new MessageModel();
                messageModel.content = message;
                messageModel.createAt = DateTime.Now;
                messageModel.senderId = userId;
                messageModel.receiverId = model.friendId;
                messageModel.isRead = false;
                //_context.MessageMxhs.Add(mes);
                //await _context.SaveChangesAsync();

                await Clients.OthersInGroup(newRoomName).SendAsync("ReceiveMessage", messageModel );
            }
        }
    }
}
