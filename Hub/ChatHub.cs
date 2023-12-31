﻿using Microsoft.AspNetCore.SignalR;
using ChatChit.Data;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using ChatChit.Models.ResponseModel;
using static System.Net.Mime.MediaTypeNames;
using ChatChit.Models.GroupChat;
using System.ComponentModel;

namespace ChatChit.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatChitContext _context;

        public ChatHub(ChatChitContext context)
        {
            _context = context;
        }
        public async Task JoinHub(string userToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(userToken) as JwtSecurityToken;

            if (token != null)
            {
                string userId = token.Claims.First(claim => claim.Type == "userId").Value;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}");
            }
        }


        public async Task JoinRoom(ChatRoomModel model)
        {
            //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", "Khoi", $"{userConnection.User} has joined {userConnection.Room}");
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(model.tokenUserId) as JwtSecurityToken;

            if (token != null)
            {
                string userId = token.Claims.First(claim => claim.Type == "userId").Value;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, model.roomId);
        }

        public async Task JoinRoomChat(ChatRoomModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(model.tokenUserId) as JwtSecurityToken;

            if (token != null)
            {
                string userId = token.Claims.First(claim => claim.Type == "userId").Value;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{userId}");
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
        public async Task SendMessage(string message, string image, ChatRoomModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(model.tokenUserId) as JwtSecurityToken;

            if (token != null)
            {
                Guid userId = Guid.Parse(token.Claims.First(claim => claim.Type == "userId").Value);

                if (string.IsNullOrEmpty(model.roomId))
                {
                    // Nếu không có roomName, đây là chat đơn
                    await SendDirectMessage(message, image, userId, model.friendId ?? Guid.NewGuid());
                }
                else
                {
                    // Nếu có roomName, đây là chat group
                    await SendGroupMessage(message, image, Guid.Parse(model.roomId), userId);
                }
            }
        }

        private async Task SendDirectMessage(string message, string image, Guid senderId, Guid receiverId)
        {
            MessageModel messageModel = new MessageModel();
            if (image != null)
            {
                messageModel.image = image;
            }
            else
            {
                messageModel.image = null;
            }
            messageModel.content = message;
            messageModel.createAt = DateTime.UtcNow;
            messageModel.senderId = senderId;
            messageModel.receiverId = receiverId;
            messageModel.isRead = false;

            var receiver = await _context.Users.FindAsync(messageModel.receiverId);
            var sender = await _context.Users.FindAsync(messageModel.senderId);

            MessageResponseModel messageresponse = new MessageResponseModel
            {
                id = messageModel.id,
                senderId = messageModel.senderId,
                receiverId = messageModel.receiverId,
                senderName = sender.nickName,
                receiverName = receiver.nickName,
                content = messageModel.content,
                image = messageModel.image,
                createAt = messageModel.createAt,
            };
            await Clients.OthersInGroup($"{senderId}{receiverId}").SendAsync("ReceiveMessage", messageresponse);

            try
            {

                _context.Messages.Add(messageModel);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error saving message to the database: {ex.Message}");
                // You might want to throw the exception here or handle it according to your application's logic
            }
            await Clients.Group(receiverId.ToString()).SendAsync("Noti", messageresponse);
        }

        private async Task SendGroupMessage(string message, string image, Guid roomId, Guid senderId)
        {
            GroupChatMessageModel messageModel = new GroupChatMessageModel();
            if (image != null)
            {
                messageModel.image = image;
            }
            else
            {
                messageModel.image = null;
            }
            messageModel.groupId = roomId;
            messageModel.content = message;
            messageModel.createAt = DateTime.UtcNow;
            messageModel.senderId = senderId;
            messageModel.isRead = false;
            var sender = await _context.Users.FindAsync(messageModel.senderId);
            MessageGroupResponseModel messageResponse = new MessageGroupResponseModel
            {
                id = messageModel.id,
                senderId = messageModel.senderId,
                senderName = sender.nickName,
                content = messageModel.content,
                image = messageModel.image,
                createAt = messageModel.createAt,
                senderImage = sender.image,
            };
            await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", messageResponse);
            _context.GroupChatMessages.Add(messageModel);
            await _context.SaveChangesAsync();
            var groupMembers = _context.GroupChatMembers
                .Where(m => m.groupId == roomId)
                .Select(m => m.userId)
                .ToList();

            foreach(Guid gm in groupMembers)
            {
                if(gm != messageModel.senderId)
                {
                await Clients.Group(gm.ToString()).SendAsync("Noti", messageResponse);
                }
            }
        }
    }
}
