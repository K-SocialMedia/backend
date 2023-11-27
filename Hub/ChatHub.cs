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
        private readonly ChatChitContext _context;

        public ChatHub(ChatChitContext context)
        {
            _context = context;
        }

        public async Task JoinRoom(string roomName)
        {
            //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", "Khoi", $"{userConnection.User} has joined {userConnection.Room}");
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task JoinRoomChat(ChatRoomModel model)
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

        //public async Task SendMessage(string message, ChatRoomModel model, string roomName)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.ReadToken(model.tokenUserId) as JwtSecurityToken;

        //    if (token != null)
        //    {
        //        string userId = token.Claims.First(claim => claim.Type == "userId").Value;

        //        string newRoomName = roomName ?? $"{userId}{model.friendId}";

        //        MessageModel messageModel = new MessageModel();
        //        messageModel.content = message;
        //        messageModel.createAt = DateTime.Now;
        //        messageModel.senderId = userId;
        //        messageModel.receiverId = model.friendId;
        //        messageModel.isRead = false;
        //        //_context.MessageMxhs.Add(mes);
        //        //await _context.SaveChangesAsync();

        //        await Clients.OthersInGroup(newRoomName).SendAsync("ReceiveMessage", messageModel);
        //    }
        //}
        public async Task SendMessage(string message, ChatRoomModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(model.tokenUserId) as JwtSecurityToken;

            if (token != null)
            {
                string userId = token.Claims.First(claim => claim.Type == "userId").Value;

                if (string.IsNullOrEmpty(model.roomId))
                {
                    // Nếu không có roomName, đây là chat đơn
                    await SendDirectMessage(message, userId, model.friendId);
                }
                else
                {
                    // Nếu có roomName, đây là chat group
                    await SendGroupMessage(message, model.roomId, userId);
                }
            }
        }

        private async Task SendDirectMessage(string message, string senderId, string receiverId)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.content = message;
            messageModel.createAt = DateTime.Now;
            messageModel.senderId = senderId;
            messageModel.receiverId = receiverId;
            messageModel.isRead = false;

            //_context.MessageMxhs.Add(messageModel);
            //await _context.SaveChangesAsync();

            await Clients.OthersInGroup($"{senderId}{receiverId}").SendAsync("ReceiveMessage", messageModel);
        }

        private async Task SendGroupMessage(string message, string roomName, string senderId)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.content = message;
            messageModel.createAt = DateTime.Now;
            messageModel.senderId = senderId;
            messageModel.isRead = false;

            //_context.MessageMxhs.Add(messageModel);
            //await _context.SaveChangesAsync();

            await Clients.OthersInGroup(roomName).SendAsync("ReceiveMessage", messageModel);
        }
    }
}
